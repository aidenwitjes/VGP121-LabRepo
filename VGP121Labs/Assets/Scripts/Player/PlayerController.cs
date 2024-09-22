using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private GameObject swordfishInstance;
    public float followSpeed = 5f;

    // Movement Variables
    [SerializeField, Range(1, 20)] private float swimSpeed = 5.0f;
    [SerializeField, Range(1, 20)] private float swimForce = 5.0f;
    [SerializeField] private float maxSinkVel = -5.0f; // Cap for sinking velocity
    [SerializeField] private float maxSwimUpVel = 10.0f; // Cap for swimming up velocity

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

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
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);
        if (!curPlayingClips[0].clip.name.Contains("VictoryDance") && !curPlayingClips[0].clip.name.Contains("Death"))
        {
            float hInput = Input.GetAxis("Horizontal");
            anim.SetFloat("hInput", Mathf.Abs(hInput));

            // Apply horizontal movement
            rb.velocity = new Vector2(hInput * swimSpeed, rb.velocity.y);

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
        else if (curPlayingClips[0].clip.name.Contains("VictoryDance") || curPlayingClips[0].clip.name.Contains("Death"))
        {
            rb.velocity = new Vector2(0, -125);
        }

        // Handle swordfish attacking mechanic
        if (swordfishInstance != null && Input.GetButtonDown("Fire1"))
        {
            // No need to call StartAttack; EnguardePowerUp handles this internally
            EnguardePowerUp swordfish = swordfishInstance.GetComponent<EnguardePowerUp>();
            if (swordfish != null)
            {
                // Attack logic is handled in EnguardePowerUp's Update method
                swordfish.isAttacking = true; // Set attacking state
            }
        }
    }

    void FixedUpdate()
    {
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

    public void SetSwordfish(GameObject swordfish)
    {
        swordfishInstance = swordfish;
    }

    public bool HasSwordfish()
    {
        return swordfishInstance != null;
    }
}