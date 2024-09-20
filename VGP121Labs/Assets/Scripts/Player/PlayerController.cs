using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    //Movement Variables
    [SerializeField, Range (1, 20)] private float swimSpeed = 5.0f;
    [SerializeField, Range(1, 20)] private float maxSwimSpeed = 10.0f;
    [SerializeField, Range(1, 20)] private float landSpeed = 7.0f;
    [SerializeField, Range(1, 20)] private float jumpForce = 5.0f;
    [SerializeField, Range(0.01f, 1)] private float groundCheckRadius = 0.02f;
    [SerializeField] private LayerMask isGroundLayer;

    [SerializeField] private float maxSinkVel = -5.0f; //Cap for sinking velocity
    [SerializeField] private float maxSwimUpVel = 10.0f; //Cap for swimming up velocity

    private Transform groundCheck;
    private bool isGrounded = false;
    private bool isUnderwater = false;
    
    //Can comment this out for final submission
    private bool swimMessage = false; //Flag to if swimming is working properly

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    //Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       sr = GetComponent<SpriteRenderer>();
       anim = GetComponent<Animator>();

        if (swimSpeed <= 0)
        {
            swimSpeed = 5;
            Debug.Log("Speed was set incorrectly");
        }
        
        if (jumpForce<= 0)
        {
            jumpForce = 5;
            Debug.Log("jumpForce was set incorrectly");
        }

        if (!groundCheck)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            groundCheck = obj.transform;
        }

        //Check the scene to determine if the player is underwater
        string sceneName = SceneManager.GetActiveScene().name;
        isUnderwater = sceneName == "Level 1";
       
        //Can comment this out for final submission
        if (SceneManager.GetActiveScene().name == "Level 1" && !swimMessage)
        {
            Debug.Log("The player is swimming.");
            swimMessage = true;
        }
        else if (SceneManager.GetActiveScene().name != "Level 1")
        {
            swimMessage = false; //Reset flag
        }

    }

    //Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        anim.SetFloat("hInput", Mathf.Abs(hInput));

        if (isUnderwater)
        {
            //Underwater input
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector2.up * swimSpeed, ForceMode2D.Impulse);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.velocity = new Vector2(rb.velocity.x, -swimSpeed); // Swim down
            }
        }
        else
        {
            //On-land input
            rb.velocity = new Vector2(hInput * landSpeed, rb.velocity.y);
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        //Flip sprite based on input direction
        if (hInput != 0) sr.flipX = (hInput < 0);
    }

    void FixedUpdate()
    {
        float hInput = Input.GetAxis("Horizontal");

        if (isUnderwater)
        {
            //Swimming movement
            rb.velocity = new Vector2(hInput * maxSwimSpeed, rb.velocity.y);

            //Cap upward swimming velocity
            if (rb.velocity.y > maxSwimUpVel)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxSwimUpVel);
            }

            //Cap sinking velocity
            if (rb.velocity.y < maxSinkVel)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxSinkVel);
            }
        }
        else
        {
            //On-land movement
            rb.velocity = new Vector2(hInput * landSpeed, rb.velocity.y);
        }
    }
}
