using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField, Range(1, 50)] private float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0)

            lifetime = 2.0f;
        Destroy(gameObject, lifetime);
    }

    public void SetVelocity(float xVel, float yVel)
    {
        float speed = 10.0f;
        GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, yVel) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collider.CompareTag("Player") && CompareTag("EnemyProjectile"))
        {
            GameManager.Instance.lives--;
            Destroy(gameObject);
        }

        if (collider.CompareTag("Enemy") && CompareTag("PlayerProjectile"))
        {
            collider.GetComponent<Enemy>().TakeDamage(10);
            Destroy(gameObject);
            Destroy(collider.gameObject, 2);
        }

        if (collider.CompareTag("PlayerProjectile") && CompareTag("EnemyProjectile"))
        {
            Destroy(gameObject);
            Destroy(collider.gameObject);
        }
    }
}

