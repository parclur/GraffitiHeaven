﻿using UnityEngine;
using Rewired;
using UnityEngine.Rendering;

public class PlayerDiverMovement : MonoBehaviour
{
    [SerializeField] private string playerAttachedToo; //Syntax is Player0, Player1, Player2, ect

    [SerializeField] private float rotateSpeed;

    [SerializeField] private float movementAcceleration; //Ramps up by this every frame

    [SerializeField] private float maxVelocity; //Maximum velocity the player can move at

    [SerializeField] private float slowedSpeed;

    [SerializeField] private int hitsToDie;


    [SerializeField] private float slowDurtaion = 2f;

    private float slowTimer = 0f;

    private bool isSlowed = false;

    [SerializeField] private float godPeriod = 2f;

    private float godTimer = 0f;

    private bool isInGodPeriod = false;

    private Rigidbody body;

    int hitCount = 0;

    private Transform playerTransform;

    private Player rewiredPlayer;





    private void Start()
    {
        body = GetComponent<Rigidbody>();
        playerTransform = gameObject.transform;
        
        rewiredPlayer = ReInput.players.GetPlayer(playerAttachedToo); //Gets the rewired players

    }

    private void Update()
    {
        HandleSlowTimer();
        HandleGodPeriod();


        if(Input.GetKeyDown(KeyCode.V))
        {
            ApplySlow();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float xAxis = rewiredPlayer.GetAxis("HorizontalAxisMOVE");
        float yAxis = rewiredPlayer.GetAxis("VerticalAxisMOVE");

        RotatePlayer(xAxis);
        HorizontalMovment(yAxis);
    }

    void RotatePlayer(float xAxis) //Will rotate the player based on the x axis of the stick
    {
        Vector3 direction = playerTransform.localEulerAngles;
        if(xAxis != 0)
            direction.y += xAxis * rotateSpeed ;
        direction.z = 0;
        direction.x = 0;

        playerTransform.localEulerAngles  = direction;
    }

    void HorizontalMovment(float yAxis) //Will add force based on the y axis of the stick
    {
        Vector3 forward = playerTransform.forward * yAxis; 
        
        if(!isSlowed)
        {
            body.AddForce(forward * movementAcceleration, ForceMode.Acceleration); //Applies movment (normal)
        }    
        else
        {
            body.AddForce(forward * slowedSpeed, ForceMode.Acceleration); //Applies movment (slowed)
        }
        
        //Cap for the velocity
        if(body.velocity.magnitude > maxVelocity)
        {
            body.velocity = body.velocity.normalized * maxVelocity;
        }

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
            PostProssesingEffectsManager.instance.DisableStaticEffect();           
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

    public void ApplySlow() //Apply's the slow if the player is not in a god period
    {
        if (!isInGodPeriod)
        { 
            isSlowed = true;
            isInGodPeriod = true;
            hitCount++;
            PostProssesingEffectsManager.instance.EnableStaticEffect();
        }

        if (hitCount >= hitsToDie)
            SceneLoader.instance.LoseGame();
    }
}