﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    // Movement Variables
    public float speed;                // Floating point variable to store the player's movement speed
    private Rigidbody2D rb2d;        // Store a reference to the Rigidbody2D component required to use 2D Physics

    // Jumping Variables
    public float jumpHeight;    // Floating point variable to store player's jumpHeight
    private bool isGrounded; // Bool that checks if player is on the ground
    public LayerMask groundLayer;  // Tags all ground objects as a layer

    // Part Variables
    private bool hasPart;
    private bool hasSexyLegs;
    private bool hasSpringLegs;
    private string isUsingNormal = "isUsingNormal";
    private string isUsingSexy = "isUsingSexy";
    private string isUsingSpring = "isUsingSpring";
    private Dictionary<string, bool> isUsing = new Dictionary<string, bool>();
    private List<string> usingKeys;

    // Use this for initialization
    void Start()
    {
        // Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();

        // Initializes a dictionary to store whether the robot is using a part or not
        isUsing = new Dictionary<string, bool>()
        {
            {isUsingNormal, true},
            {isUsingSexy, false},
            {isUsingSpring, false},
        };

        // List of keys for iteration
        usingKeys = new List<string>(isUsing.Keys);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), groundLayer);

        // Jump Input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            defaultJump(rb2d, jumpHeight);
        }
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        // Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        // Unnecessary Code
        /*// Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        // Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        rb2d.AddForce(movement * speed); */

        // Left Input
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
        }

        // Z input -> Use normal limbs
        if (Input.GetKeyDown(KeyCode.Z))
        {
            limbUpdate(isUsingNormal);
        }

        // X input -> Use sexy limbs
        if (Input.GetKeyDown(KeyCode.X) && hasSexyLegs)
        {
            limbUpdate(isUsingSexy);
        }

        // C input -> Use spring limbs
        if (Input.GetKeyDown(KeyCode.C) && hasSpringLegs)
        {
            limbUpdate(isUsingSpring);
        }
    }

    // Default Jump Function
    private void defaultJump(Rigidbody2D rb2d, float jumpHeight)
    {
        rb2d.velocity = Vector2.up * jumpHeight;
    }

    private void limbUpdate(string limb)
    {
        foreach(string i in usingKeys)
        {
            if(i == limb)
            {
                Debug.Log(isUsing[i]);
                isUsing[i] = true;
            }
            else
            {
                isUsing[i] = false;
            }
        } 
    }

    // Collider function
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            jumpHeight *= 2;
        }

        if (other.gameObject.CompareTag("SexyLegs"))
        {
            Debug.Log("Has sexy legs!");
            hasSexyLegs = true;
        }

        if (other.gameObject.CompareTag("SpringLegs"))
        {
            Debug.Log("Has spring legs!");
            hasSpringLegs = true;
        }

        other.gameObject.SetActive(false);
    }

}
