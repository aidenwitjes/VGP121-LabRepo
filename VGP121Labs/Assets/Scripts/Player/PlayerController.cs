using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Range (1, 20)] private float speed = 5;
    [SerializeField, Range(1, 20)] private float swimForce = 5;
    [SerializeField, Range(0.01f, 1)] private float groundCheckRadius = 0.02f;
    [SerializeField] private LayerMask isGroundLayer;

    private Transform groundCheck;
    private bool isGrounded = false;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       sr = GetComponent<SpriteRenderer>();
       anim = GetComponent<Animator>();

        if (speed <= 0)
        {
            speed = 5;
            Debug.Log("Speed was set incorrectly");
        }
        
        if (swimForce<= 0)
        {
            swimForce = 5;
            Debug.Log("SwimForce was set incorrectly");
        }

        if (!groundCheck)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            groundCheck = obj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(hInput * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * swimForce, ForceMode2D.Impulse);
        }

        if (hInput != 0) sr.flipX = (hInput < 0);

        anim.SetFloat("hInput", Mathf.Abs(hInput));
    }
}
