﻿using UnityEngine;
using Rewired;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerDiverMovement : MonoBehaviour
{
    [SerializeField] private string playerAttachedToo; //Syntax is Player0, Player1, Player2, ect

    [SerializeField] private float rotateSpeed;

    [SerializeField] private float fallingRotateSpeed;

    [SerializeField] private float movementAcceleration; //Ramps up by this every frame

    [SerializeField] private float maxVelocity; //Maximum velocity the player can move at

    [SerializeField] private float slowedSpeed;

    [SerializeField] private float fallingHorizontalSpeed;

    [SerializeField] private int hitsToDie;

    [SerializeField] private float slowDurtaion = 4f;

    [SerializeField] private bool doCC;

    private float slowTimer = 0f;

    private bool isSlowed = false;

    private bool isGrounded = true;

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
        CheckGrounded();
        MovePlayer();
    }

    void MovePlayer()
    {
        float xAxis = rewiredPlayer.GetAxis("HorizontalAxisMOVE");
        float yAxis = rewiredPlayer.GetAxis("VerticalAxisMOVE");

        //Handles animation variables
        if(isGrounded)
        {
            anim.SetFloat("Forward", yAxis);
            anim.SetFloat("Turn", xAxis);
        }
        else
        {
            anim.SetFloat("Forward", yAxis /2);
            anim.SetFloat("Turn", xAxis /2);
        }

        RotatePlayer(xAxis);
        HorizontalMovment(yAxis);
    }

    void RotatePlayer(float xAxis) //Will rotate the player based on the x axis of the stick
    {
        Vector3 direction = playerTransform.localEulerAngles;

        if(isGrounded) //If the player is grounded roate normally
        {
            if(xAxis != 0)
                direction.y += xAxis * rotateSpeed ;
            direction.z = 0;
            direction.x = 0;
        }
        else //If the player is not grounded roate at a diffrent speed
        {
            if(xAxis != 0)
                direction.y += xAxis * fallingRotateSpeed ;
            direction.z = 0;
            direction.x = 0;
        }

        playerTransform.localEulerAngles  = direction;
    }

    void HorizontalMovment(float yAxis) //Will add force based on the y axis of the stick
    {
        float acceleration = movementAcceleration; //Sets accleration to it's default value (will overrite if needed)
        Vector3 forward = playerTransform.forward * yAxis; 
        //bool isGrounded = CheckGrounded();

        // if(!isSlowed && isGrounded) //If it is not slowed & is grounded, 
        // {
        //     acceleration = movementAcceleration; //Applies movment (normal)
        // }    
        if(!isGrounded) //Checks to see if the player is not grounded (Takes priority over slowed)
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

    void CheckGrounded() //Limits the player's movment while they are in the air
    {
        //An altered lowerbound of the feet colider to be slightly underneith to account for some slight inconsistancies in the floor
        Vector3 endPointAltered = new Vector3(feetCollider.bounds.center.x, 
        feetCollider.bounds.min.y - 0.1f, feetCollider.bounds.center.z);

        //This will check a capsule slightly under the player's feet, and return if it is grounded or not
        //Set to layermask 11, this layer is set to ignore objsticals and the player for collisions
        isGrounded = Physics.CheckCapsule(feetCollider.bounds.center, endPointAltered, .18f, 11);

        //Debug spam for debugging purposoes! 
        if (isGrounded)
            Debug.Log("Is grounded!");
        else
            Debug.Log("Isn't groudned!");


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

    public List<GameObject> getKeyList()
    {
        return keys;
    }

    public void addKey(GameObject toBeAdded)
    {
        keys.Add(toBeAdded);
    }
}