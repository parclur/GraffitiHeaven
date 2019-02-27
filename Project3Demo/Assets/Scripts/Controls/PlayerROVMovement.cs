using UnityEngine;
using Rewired;

public class PlayerROVMovement : MonoBehaviour
{
    [Header("Movement Variables")]

    [SerializeField] private string playerControlling; //Syntax "Player0, Player1, Player2, ect"

    [SerializeField] private float playerDirectionalSpeed;

    [SerializeField] private float playerRiseDesendSpeed;

    [SerializeField] private float playerVerticalRotateSpeed;

    [SerializeField] private float playerHorizontalRotateSpeed;

    private Player rewiredPlayer;

    private Transform playerTransform;

    private Rigidbody playerRidgedBody;

    private void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer(playerControlling); //Gets the rewire player
        playerRidgedBody = gameObject.GetComponent<Rigidbody>();
        playerTransform = gameObject.transform;
    }

    private void Update()
    {
        ReadInput();
    }

    void ReadInput() //Interperates the input of the player
    {
        //----------------------------------------MOVE DOWN----------------------------------------------

        if(rewiredPlayer.GetButtonLongPress("ElevateDOWN")) 
        {
            MoveDown();
        }

        //----------------------------------------MOVE UP----------------------------------------------

        if(rewiredPlayer.GetButtonLongPress("ElevateUP"))
        {
            MoveUp();
        }

        //----------------------------------------ROTATE AXIS----------------------------------------------
        float xAxisAim = rewiredPlayer.GetAxis("UpDownAim");
        float yAxisAim = rewiredPlayer.GetAxis("LeftRightAim");

        RotatePlayer(xAxisAim, yAxisAim);

        //----------------------------------------MOVE AXIS----------------------------------------------
        float yAxisMove = rewiredPlayer.GetAxis("UpDownMovment");
        float xAxisMove = rewiredPlayer.GetAxis("LeftRightMovement");

        MoveDirectional(xAxisMove, yAxisMove);
    }

    void MoveUp() //Moves them directly up (based on singular button input)
    {
        Vector3 direction = new Vector3(0,1 * playerRiseDesendSpeed,0);

        playerRidgedBody.AddForce(direction);
    }

    void MoveDown() //Moves them directly down (based on singular button input)
    {
        Vector3 direction = new Vector3(0,-1 * playerRiseDesendSpeed,0);

        playerRidgedBody.AddForce(direction);
    }

    void MoveDirectional(float xAxis, float yAxis) //Moves them in a direction based on the axis input
    {
        Vector3 right = new Vector3(playerTransform.right.x, 0, playerTransform.right.z) * xAxis;
        Vector3 forward = new Vector3(playerTransform.forward.x, 0, playerTransform.forward.z) * yAxis;
        Vector3 direction = right + forward;

        playerRidgedBody.AddForce(direction * playerDirectionalSpeed);
    }

    void RotatePlayer(float xAxis, float yAxis) //Rotates them in a dirction based on the axis input
    {
        Vector3 direction = playerTransform.localEulerAngles;
        if(xAxis != 0)
            direction.x += xAxis * playerHorizontalRotateSpeed * -1;//* Time.time;
        if(yAxis != 0)
            direction.y += yAxis * playerVerticalRotateSpeed ;//* Time.time;
        direction.z = 0;

        playerTransform.localEulerAngles  = direction;
    }
}