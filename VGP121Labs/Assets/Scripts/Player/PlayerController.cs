using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Movement Variables
    [SerializeField, Range(1, 20)] private float swimSpeed = 5.0f;
    [SerializeField, Range(1, 20)] private float maxSwimSpeed = 10.0f;
    [SerializeField, Range(1, 20)] private float swimForce = 5.0f;
    [SerializeField] private float maxSinkVel = -5.0f; // Cap for sinking velocity
    [SerializeField] private float maxSwimUpVel = 10.0f; // Cap for swimming up velocity

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (swimSpeed <= 0)
        {
            swimSpeed = 5;
            Debug.Log("Swim speed was set incorrectly, defaulting to 5.");
        }

        if (swimForce <= 0)
        {
            swimForce = 5;
            Debug.Log("Swim force was set incorrectly, defaulting to 5.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        anim.SetFloat("hInput", Mathf.Abs(hInput));

        // Swim up when Jump is pressed
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * swimForce, ForceMode2D.Impulse);
        }

        // Swim down when Down Arrow is pressed
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, -swimSpeed); // Swim down
        }

        // Flip sprite based on input direction
        if (hInput != 0)
        {
            sr.flipX = (hInput < 0);
        }
    }

    void FixedUpdate()
    {
        float hInput = Input.GetAxis("Horizontal");

        // Apply horizontal movement
        rb.velocity = new Vector2(hInput * swimSpeed, rb.velocity.y);

        // Cap upward swimming velocity
        if (rb.velocity.y > maxSwimUpVel)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxSwimUpVel);
        }

        // Cap sinking velocity
        if (rb.velocity.y < maxSinkVel)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxSinkVel);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.lives--;
        }
    }
}