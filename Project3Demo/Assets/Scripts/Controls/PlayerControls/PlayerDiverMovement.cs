using UnityEngine;
using Rewired;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerDiverMovement : MonoBehaviour
{

    //--------------------------------------------------Public variables-------------------------------------------------

    [SerializeField] private string playerAttachedToo; //Syntax is Player0, Player1, Player2, ect

    [Header("Speed vairables")]

    [SerializeField] private float rotateSpeed;

    [SerializeField] private float fallingRotateSpeed;

    [SerializeField] private float movingRotateSpeed; //Speed the diver moves while moving forward / backwards

    [SerializeField] private float movementAcceleration; //Ramps up by this every frame

    [SerializeField] private float slowedSpeed;

    [SerializeField] private float fallingHorizontalSpeed;

    [Header("Health Variables")]

    [SerializeField] private int hitsToDie;

    [SerializeField] private float slowDurtaion = 4f;

    [SerializeField] private float godPeriod = 2f;

    [Header("Rotation Controlls")]

    [SerializeField] private float rotationDistanceForForwardMovment; //The minimum angle for forward movment

    //----------------------------------------Timer variables-------------------------------

    private float slowTimer = 0f;

    private float godTimer = 0f;

    //-----------------------------------------Bools----------------------------------------

    private bool isSlowed = false;

    [HideInInspector] public bool isGrounded = true;

    private bool isMovingHorizontal = false;
 
    private bool isInGodPeriod = false;

    //-----------------------------------------Componants-------------------------------------

    CharacterController cc;

    private Animator anim;

    private Transform playerTransform;

    private Player rewiredPlayer;

    private CapsuleCollider feetCollider;

    //---------------------------------------Physics------------------------------------------

    private Vector3 gravity = Physics.gravity / 130;

    private float distanceToGround;

    //--------------------------------------Keys---------------------------------------------

    List<GameObject> keys;

    //-------------------------------------HP-----------------------------------------------

    int hitCount = 0;

    //------------------------------------Controller Layouts--------------------------------

    private enum ControllerMaps
    {
        LayoutA, LayoutB, LayoutC
    };

    private ControllerMaps currentControllerMap = ControllerMaps.LayoutC;

    private Camera cameraMain;
    private Transform cameraTransform;

    private void Start()
    {
        Cursor.visible = false;
        keys = new List<GameObject>();
        cc = GetComponent<CharacterController>();

        
        playerTransform = gameObject.transform;
        
        rewiredPlayer = ReInput.players.GetPlayer(playerAttachedToo); //Gets the rewired players
        anim = GetComponent<Animator>();

        //distanceToGround = GetComponent<Collider>().bounds.extents.y;

        feetCollider = gameObject.GetComponent<CapsuleCollider>();

        cameraMain = Camera.main;
        cameraTransform = cameraMain.transform;

        //Sets the player to layout C
        rewiredPlayer.controllers.maps.LoadMap(ControllerType.Joystick, rewiredPlayer.controllers.Joysticks[0].id, "Default", "DiverC");//Ensures new layout is loaded
        rewiredPlayer.controllers.maps.SetAllMapsEnabled(false);//Disables all previous layouts
        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverC");//Enable new layout
        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverLayoutKey");//Enable keyboard controlls
        currentControllerMap = ControllerMaps.LayoutC;
    }

    private void Update()
    {
        HandleSlowTimer();
        HandleGodPeriod();

        if (Input.GetKeyDown(KeyCode.V))
        {
            ApplySlow();
        }
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        MovePlayer();
    }

    void MovePlayer()
    {
        float yAxis = rewiredPlayer.GetAxis("UpDownMovment");
        float xAxisMove = rewiredPlayer.GetAxis("LeftRightMovement");
        float xAxisAim = rewiredPlayer.GetAxis("LeftRightAim");
        float xAxis = xAxisMove;

        if(xAxisAim != 0) //When using layout 2 the aiming will have AIM axis be an override when being used (will not effect other layouts)
        {
            xAxis = xAxisAim;
        }


        if(currentControllerMap == ControllerMaps.LayoutC)
        {
            ThreeDirectionalMovment(xAxis, yAxis);
        }
        //else
        //{
        //    RotatePlayer(xAxis);
        //    HorizontalMovment(yAxis);

        //    //Handles animation variables
        //    if (isGrounded)
        //    {
        //        anim.SetFloat("Forward", yAxis);
        //        anim.SetFloat("Turn", xAxis);
        //    }
        //    else
        //    {
        //        anim.SetFloat("Forward", yAxis / 2);
        //        anim.SetFloat("Turn", xAxis / 2);
        //    }
        //}

    }

    void ThreeDirectionalMovment(float xAxis, float yAxis)
    {
        Vector3 forward= cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        float acceleration = 0; 
        float distanceToRotate = 0f;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * yAxis + right * xAxis;
        if(desiredMoveDirection.x != 0 || desiredMoveDirection.y != 0 || desiredMoveDirection.z != 0) //If the player needs to be rotated...
        {
            distanceToRotate = RotateTowardsPoint(desiredMoveDirection); //Will update the distance to rotate if rotation is required

        }

        if (distanceToRotate < rotationDistanceForForwardMovment && yAxis != 0 || xAxis != 0) //If the player is able to move forward
        {
            acceleration = movementAcceleration; //Sets accleration to it's default value (will overrite if needed)

            if (!isGrounded) //Checks to see if the player is not grounded (Takes priority over slowed)
            {
                acceleration = fallingHorizontalSpeed; //Appleis movment (falling)
            }
            else if (isSlowed) //Checks to see if the player is slowed, then applies new accleration
            {
                acceleration = slowedSpeed; //Applies movment (slowed)
            }

        }

        cc.Move(desiredMoveDirection * acceleration + gravity);

        //Debug.Log(acceleration);
        HandleAnimations(acceleration * 100, distanceToRotate);
    }

    float RotateTowardsPoint(Vector3 rotationTarget) //Will return the distance that is required to reach the inputed rotation
    {
        float output = 0f;

        Vector3 rotation = Vector3.RotateTowards(playerTransform.forward, rotationTarget, rotateSpeed, 0.0f); //Gets the rotation target

        playerTransform.rotation = Quaternion.LookRotation(rotation); //Adjusts the rotation

        float angle = Vector3.Angle(rotationTarget, playerTransform.forward); //Calculates the angle of the direction and the target

        output = angle;

        return output;
    }

    void HandleSlowTimer() //Handles the logic for the slow timer. To start the timer set isSlowed to true
    {
        if(isSlowed)
            slowTimer += Time.deltaTime;

        if (isSlowed && slowTimer > slowDurtaion)
        {
            slowTimer = 0f;
            isSlowed = false;
            hitCount = 0;
            PostProcessingManager.instance.DisableStaticEffect();           
        }
    }

    void HandleGodPeriod() //Handles the logic for the godPeriod timer. To start the timer set isInGodPeriod to false
    {
        if(isInGodPeriod)
            godTimer += Time.deltaTime;

        if (isInGodPeriod && godTimer > godPeriod)
        {
            godTimer = 0f;
            isInGodPeriod = false;
        }
    }

    void CheckGrounded() //Limits the player's movment while they are in the air
    {
        //An altered lowerbound of the feet colider to be slightly underneith to account for some slight inconsistancies in the floor
        Vector3 endPointAltered = new Vector3(feetCollider.bounds.center.x, 
        feetCollider.bounds.min.y - 0.1f, feetCollider.bounds.center.z);

        //This will check a capsule slightly under the player's feet, and return if it is grounded or not
        //Set to layermask 11, this layer is set to ignore objsticals and the player for collisions
        isGrounded = Physics.CheckCapsule(feetCollider.bounds.center, endPointAltered, .18f, 11);

    }


    public void ApplySlow() //Apply's the slow if the player is not in a god period
    {
        if (!isInGodPeriod)
        { 
            isSlowed = true;
            isInGodPeriod = true;
            hitCount++;
            PostProcessingManager.instance.EnableStaticEffect();
        }

        if (hitCount >= hitsToDie)
            SceneLoader.instance.LoseGame();
    }

    public List<GameObject> getKeyList()
    {
        return keys;
    }

    public void addKey(GameObject toBeAdded)
    {
        keys.Add(toBeAdded);
    }

    void HandleAnimations(float acceleration, float turnSpeed)
    {
        //Finds the absolute value of the imputed acceleration, NOTE: this will break backwards movment however there is currently none implemented
        float accelerationABS = Mathf.Abs(acceleration); 

        //Handle rotational animations
        if (isGrounded)
        {
            anim.SetFloat("Turn", turnSpeed);
        }
        else
        {
            anim.SetFloat("Turn", turnSpeed / 2);
        }

        //Find current speed of player
        if (isGrounded)
        {
            anim.SetFloat("Forward", accelerationABS);
        }
        else
        {
            anim.SetFloat("Forward", accelerationABS / 2);
        }
    }
    //-----------------------------------Legacy movment code, used for refrance and for layout 2 when we eventally allow controll swaping--------------------------

    //void RotatePlayer(float xAxis) //Will rotate the player based on the x axis of the stick
    //{
    //    Vector3 direction = playerTransform.localEulerAngles;

    //    if (isGrounded) //If the player is grounded roate normally
    //    {
    //        if (isMovingHorizontal)
    //        {
    //            if (xAxis != 0)
    //            {
    //                direction.y += xAxis * movingRotateSpeed;
    //            }
    //            direction.z = 0;
    //            direction.x = 0;
    //        }
    //        else
    //        {
    //            if (xAxis != 0)
    //            {
    //                direction.y += xAxis * rotateSpeed;
    //            }
    //            direction.z = 0;
    //            direction.x = 0;
    //        }

    //    }
    //    else //If the player is not grounded roate at a diffrent speed
    //    {
    //        if (xAxis != 0)
    //        {
    //            direction.y += xAxis * fallingRotateSpeed;
    //        }
    //        direction.z = 0;
    //        direction.x = 0;
    //    }

    //    playerTransform.localEulerAngles = direction;
    //}

    //void HorizontalMovment(float yAxis) //Will add force based on the y axis of the stick
    //{
    //    float acceleration = movementAcceleration; //Sets accleration to it's default value (will overrite if needed)
    //    Vector3 forward = playerTransform.forward * yAxis;

    //    if (!isGrounded) //Checks to see if the player is not grounded (Takes priority over slowed)
    //    {
    //        acceleration = fallingHorizontalSpeed; //Appleis movment (falling)
    //    }
    //    else if (isSlowed) //Checks to see if the player is slowed, then applies new accleration
    //    {
    //        acceleration = slowedSpeed; //Applies movment (slowed)
    //    }

    //    cc.Move(forward * acceleration + gravity);


    //    if (forward.x != 0.0 && forward.z != 0) //If player is moving in either z or x direction
    //    {
    //        isMovingHorizontal = true;
    //    }
    //    else
    //    {
    //        isMovingHorizontal = false;
    //    }
    //}


    //void HandleControllerMapSwaping() //NOTE: Used for swaping controller map, can be cut later if feature not need after QA
    //{
    //    if (rewiredPlayer.GetButtonDown("MoveToLayout1"))
    //    {
    //        rewiredPlayer.controllers.maps.LoadMap(ControllerType.Joystick, rewiredPlayer.controllers.Joysticks[0].id, "Default", "DiverA"); //Ensures new layout is loaded
    //        rewiredPlayer.controllers.maps.SetAllMapsEnabled(false); //Disables all previous layouts
    //        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverA"); //Enable new layout
    //        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverLayoutKey"); //Enable keyboard controlls
    //        currentControllerMap = ControllerMaps.LayoutA;
    //        Debug.Log("CurrentLayout: A");
    //        outputText.text = "LayoutA";
    //    }
    //    if (rewiredPlayer.GetButtonDown("MoveToLayout2"))
    //    {
    //        rewiredPlayer.controllers.maps.LoadMap(ControllerType.Joystick, rewiredPlayer.controllers.Joysticks[0].id, "Default", "DiverB");//Ensures new layout is loaded
    //        rewiredPlayer.controllers.maps.SetAllMapsEnabled(false);//Disables all previous layouts
    //        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverB");//Enable new layout
    //        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverLayoutKey");//Enable keyboard controlls
    //        currentControllerMap = ControllerMaps.LayoutB;
    //        Debug.Log("CurrentLayout: B");
    //        outputText.text = "LayoutB";
    //    }
    //    if (rewiredPlayer.GetButtonDown("MoveToLayout3"))
    //    {
    //        rewiredPlayer.controllers.maps.LoadMap(ControllerType.Joystick, rewiredPlayer.controllers.Joysticks[0].id, "Default", "DiverC");//Ensures new layout is loaded
    //        rewiredPlayer.controllers.maps.SetAllMapsEnabled(false);//Disables all previous layouts
    //        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverC");//Enable new layout
    //        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverLayoutKey");//Enable keyboard controlls
    //        currentControllerMap = ControllerMaps.LayoutC;
    //        Debug.Log("CurrentLayout: C");
    //        outputText.text = "LayoutC";
    //    }
    //}

}