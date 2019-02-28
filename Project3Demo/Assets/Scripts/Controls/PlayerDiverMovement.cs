using UnityEngine;
using Rewired;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerDiverMovement : MonoBehaviour
{
    [SerializeField] private string playerAttachedToo; //Syntax is Player0, Player1, Player2, ect

    [SerializeField] private float rotateSpeed;

    [SerializeField] private float fallingRotateSpeed;

    [SerializeField] private float movingRotateSpeed; //Speed the diver moves while moving forward / backwards

    [SerializeField] private float movementAcceleration; //Ramps up by this every frame

    [SerializeField] private float maxVelocity; //Maximum velocity the player can move at

    [SerializeField] private float slowedSpeed;

    [SerializeField] private float fallingHorizontalSpeed;

    [SerializeField] private int hitsToDie;

    [SerializeField] private float slowDurtaion = 4f;

    [SerializeField] private bool doCC;

    private float slowTimer = 0f;

    private bool isSlowed = false;

    public bool isGrounded = true;

    private bool isMovingHorizontal = false;

    [SerializeField] private float godPeriod = 2f;

    private float godTimer = 0f;

    private bool isInGodPeriod = false;

    CharacterController cc;

    int hitCount = 0;

    private Transform playerTransform;

    private Player rewiredPlayer;

    private Vector3 gravity = Physics.gravity / 250;

    private Animator anim;

    private Rigidbody rb;

    private float distanceToGround;

    private CapsuleCollider feetCollider;

    List<GameObject> keys;

    //For controller test during QA, unless we decide that we want multiple maps for final product this can be cut in the future
    //------------------------------------------------------------
    private enum ControllerMaps
    {
        LayoutA, LayoutB, LayoutC
    };

    private ControllerMaps currentControllerMap = ControllerMaps.LayoutA;

    private Camera cameraMain;
    private Transform cameraTransform;
    [SerializeField] Text outputText;

    //------------------------------------------------------------

    private void Start()
    {
        keys = new List<GameObject>();
        if(doCC){
            cc = GetComponent<CharacterController>();
        }
        else {
            rb = GetComponent<Rigidbody>();
        }
        
        playerTransform = gameObject.transform;
        
        rewiredPlayer = ReInput.players.GetPlayer(playerAttachedToo); //Gets the rewired players
        anim = GetComponent<Animator>();

        //distanceToGround = GetComponent<Collider>().bounds.extents.y;

        feetCollider = gameObject.GetComponent<CapsuleCollider>();

        cameraMain = Camera.main;
        cameraTransform = cameraMain.transform;

        //rewiredPlayer.controllers.maps.LoadMap(ControllerType.Joystick, rewiredPlayer.controllers.Joysticks[0].id, "Default", "DiverA");
        //currentControllerMap = ControllerMaps.LayoutA;
    }

    private void Update()
    {
        HandleSlowTimer();
        HandleGodPeriod();
        HandleControllerMapSwaping();

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

        //Handles animation variables
        if(isGrounded)
        {
            anim.SetFloat("Forward", yAxis / 1.15f);
            anim.SetFloat("Turn", xAxis / 1.15f);
        }
        else
        {
            anim.SetFloat("Forward", yAxis /2);
            anim.SetFloat("Turn", xAxis /2);
        }

        if(currentControllerMap == ControllerMaps.LayoutC)
        {
            ThreeDirectionalMovment(xAxis, yAxis);
        }
        else
        {
            RotatePlayer(xAxis);
            HorizontalMovment(yAxis);
        }

    }

    void RotatePlayer(float xAxis) //Will rotate the player based on the x axis of the stick
    {
        Vector3 direction = playerTransform.localEulerAngles;

        if(isGrounded) //If the player is grounded roate normally
        {
            if(isMovingHorizontal)
            {
                if (xAxis != 0)
                {
                    direction.y += xAxis * movingRotateSpeed;
                }
                direction.z = 0;
                direction.x = 0;
            }
            else
            {
                if (xAxis != 0)
                {
                    direction.y += xAxis * rotateSpeed;
                }
                direction.z = 0;
                direction.x = 0;
            }

        }
        else //If the player is not grounded roate at a diffrent speed
        {
            if(xAxis != 0)
            {
                direction.y += xAxis * fallingRotateSpeed;
            }
            direction.z = 0;
            direction.x = 0;
        }

        playerTransform.localEulerAngles  = direction;
    }

    void HorizontalMovment(float yAxis) //Will add force based on the y axis of the stick
    {
        float acceleration = movementAcceleration; //Sets accleration to it's default value (will overrite if needed)
        Vector3 forward = playerTransform.forward * yAxis;

        if (!isGrounded) //Checks to see if the player is not grounded (Takes priority over slowed)
        {
            acceleration = fallingHorizontalSpeed; //Appleis movment (falling)
        }
        else if (isSlowed) //Checks to see if the player is slowed, then applies new accleration
        {
            acceleration = slowedSpeed; //Applies movment (slowed)
        }

        if (doCC)
        {
            cc.Move(forward * acceleration + gravity);
        }
        else {
            rb.AddForce(forward * acceleration, ForceMode.Acceleration);

            //Cap for the velocity
            if(rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }
        }

        if (forward.x != 0.0 && forward.z != 0) //If player is moving in either z or x direction
        {
            isMovingHorizontal = true;
        }
        else
        {
            isMovingHorizontal = false;
        }
    }

    void ThreeDirectionalMovment(float xAxis, float yAxis)
    {
        Vector3 forward= cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        float acceleration = movementAcceleration; //Sets accleration to it's default value (will overrite if needed)

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        if (!isGrounded) //Checks to see if the player is not grounded (Takes priority over slowed)
        {
            acceleration = fallingHorizontalSpeed; //Appleis movment (falling)
        }
        else if (isSlowed) //Checks to see if the player is slowed, then applies new accleration
        {
            acceleration = slowedSpeed; //Applies movment (slowed)
        }

        Vector3 desiredMoveDirection = forward * yAxis + right * xAxis;
        if(desiredMoveDirection.x != 0 || desiredMoveDirection.y != 0 || desiredMoveDirection.z != 0)
        {
            transform.rotation = Quaternion.LookRotation(desiredMoveDirection);
        }

        cc.Move(desiredMoveDirection * acceleration + gravity);
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

        //Debug spam for debugging purposoes! 
        //if (isGrounded)
        //    Debug.Log("Is grounded!");
        //else
        //    Debug.Log("Isn't groudned!");


    }

    void HandleControllerMapSwaping() //NOTE: Used for swaping controller map, can be cut later if feature not need after QA
    {
        if(rewiredPlayer.GetButtonDown("MoveToLayout1"))
        {
            rewiredPlayer.controllers.maps.LoadMap(ControllerType.Joystick, rewiredPlayer.controllers.Joysticks[0].id, "Default", "DiverA"); //Ensures new layout is loaded
            rewiredPlayer.controllers.maps.SetAllMapsEnabled(false); //Disables all previous layouts
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverA"); //Enable new layout
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverLayoutKey"); //Enable keyboard controlls
            currentControllerMap = ControllerMaps.LayoutA;
            Debug.Log("CurrentLayout: A");
            outputText.text = "LayoutA";
        }
        if (rewiredPlayer.GetButtonDown("MoveToLayout2"))
        {
            rewiredPlayer.controllers.maps.LoadMap(ControllerType.Joystick, rewiredPlayer.controllers.Joysticks[0].id, "Default", "DiverB");//Ensures new layout is loaded
            rewiredPlayer.controllers.maps.SetAllMapsEnabled(false);//Disables all previous layouts
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverB");//Enable new layout
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverLayoutKey");//Enable keyboard controlls
            currentControllerMap = ControllerMaps.LayoutB;
            Debug.Log("CurrentLayout: B");
            outputText.text = "LayoutB";
        }
        if (rewiredPlayer.GetButtonDown("MoveToLayout3"))
        {
            rewiredPlayer.controllers.maps.LoadMap(ControllerType.Joystick, rewiredPlayer.controllers.Joysticks[0].id, "Default", "DiverC");//Ensures new layout is loaded
            rewiredPlayer.controllers.maps.SetAllMapsEnabled(false);//Disables all previous layouts
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverC");//Enable new layout
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default", "DiverLayoutKey");//Enable keyboard controlls
            currentControllerMap = ControllerMaps.LayoutC;
            Debug.Log("CurrentLayout: C");
            outputText.text = "LayoutC";
        }
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

}