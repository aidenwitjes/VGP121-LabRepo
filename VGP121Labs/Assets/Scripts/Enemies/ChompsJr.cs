using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class ChompsJr : Enemy
{
    [SerializeField] private float speed;
    public float amplitude = 1.0f;
    public float frequency = 2.0f;

    Rigidbody2D rb;
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (speed <= 0) speed = 10.0f;
    }

    void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        float wave = Mathf.Sin(Time.time * frequency) * amplitude;

        if (curPlayingClips[0].clip.name.Contains("ChompsJr_Swim"))
        {
            float xVel = (sr.flipX) ? -speed : speed;
            rb.velocity = new Vector2(xVel, wave);
        }
        else if (curPlayingClips[0].clip.name.Contains("ChompsJr_Death"))
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
    public void FlipSprite()
    {
        sr.flipX = !sr.flipX;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ChompsJrBarrier"))
        {
            anim.SetTrigger("TurnAround");
        }
    }
}