using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController: MonoBehaviour
{
    // Movement Variables
    public float speed;                // Floating point variable to store the player's movement speed
    private float defaultSpeed;
    private Rigidbody2D rb2d;        // Store a reference to the Rigidbody2D component required to use 2D Physics
    public Animator anim;

    // Jumping Variables
    public float jumpHeight;    // Floating point variable to store player's jumpHeight
    public bool isGrounded; // Bool that checks if player is on the ground
    public LayerMask groundLayer;  // Tags all ground objects as a layer

    // Part Variables
    private bool hasSexy = false;
    private bool hasSpring = false;
    private string isUsingNormal = "isUsingNormal";
    private string isUsingSexy = "isUsingSexy";
    private string isUsingSpring = "isUsingSpring";
    private Dictionary<string, bool> isUsing;
    private List<string> usingKeys;

    // Use this for initialization
    void Start()
    {
        // Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Store default speed value
        defaultSpeed = speed;

        // Define dictionary
        isUsing = new Dictionary<string, bool>()
        {
            {isUsingNormal, true},
            {isUsingSexy, false},
            {isUsingSpring, false},
        };

        //Define keys
        usingKeys = new List<string>(isUsing.Keys);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), groundLayer);

        // Jump Input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // If it has Sexy Legs and is using Sexy Legs
            if (hasSexy && isUsing[isUsingSexy])
            {
                Jump(rb2d, jumpHeight/2);
            // If it has Spring Legs and is using Spring Legs
            } else if (hasSpring && isUsing[isUsingSpring])
            {
                Jump(rb2d, jumpHeight * 2);
             // Else, it uses the default settings
            } else
            {
                Jump(rb2d, jumpHeight);
            }
        }

        // Z Input -> Normal Input
        if (Input.GetKeyDown(KeyCode.Z))
        {
            limbUpdate(isUsingNormal);
        }

        // X Input -> Sexy Input
        if (Input.GetKeyDown(KeyCode.X) && hasSexy)
        {
            limbUpdate(isUsingSexy);
        }

        // C Input -> Spring Input
        if (Input.GetKeyDown(KeyCode.C) && hasSpring)
        {
            limbUpdate(isUsingSpring);
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

        // Left/Right Arrow Input
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if(hasSexy && isUsing[isUsingSexy])
            {
                speed = defaultSpeed * 2;
            } else if (hasSpring && isUsing[isUsingSpring])
            {
                if (isGrounded)
                {
                    Jump(rb2d, jumpHeight / 2);
                }
                speed = defaultSpeed / 2;
            } else
            {
                speed = defaultSpeed;
            }
            rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
        }

    }

    // Default Jump Function
    private void Jump(Rigidbody2D rb2d, float jumpHeight)
    {
        rb2d.velocity = Vector2.up * jumpHeight;
    }


    private void limbUpdate(string limb)
    {
        foreach(string current in usingKeys)
        {
            if (current.Equals(limb))
            {
                Debug.Log(current);
                isUsing[current] = true;
            } else
            {
                isUsing[current] = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SexyLegs"))
        {
            hasSexy = true;
        } else if (other.gameObject.CompareTag("SpringLegs"))
        {
            hasSpring = true;
        }

        other.gameObject.SetActive(false);
    }

}