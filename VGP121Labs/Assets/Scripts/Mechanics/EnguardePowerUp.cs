using UnityEngine;
using System.Collections;

public class EnguardePowerUp : MonoBehaviour
{
    public float followSpeed = 5f; // Speed at which the Enguarde follows the player or attacks
    public float attackRange = 2f; // Distance to trigger the attack
    public bool isAttacking = false; // Flag to determine if Enguarde is attacking

    private Transform player; // Reference to the player (Donkey Kong)

    private void Start()
    {
        player = GameManager.Instance.PlayerInstance.transform;
    }

    private void Update()
    {
        // Check for attack input
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true; // Set attacking state
        }

        if (isAttacking)
        {
            MoveToNearestEnemy();
        }
        else
        {
            OrbitPlayer();
        }
    }

    private void OrbitPlayer()
    {
        // Implement the orbiting logic around the player
        Vector3 direction = (transform.position - player.position).normalized;
        float orbitDistance = 1.5f; // Distance to maintain from the player
        Vector3 targetPosition = player.position + direction * orbitDistance;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    private void MoveToNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy")) // Assuming your enemies have this tag
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null && !enemyScript.IsInvulnerable())
                {
                    float distance = Vector2.Distance(transform.position, enemy.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy.gameObject;
                    }
                }
            }
        }

        if (closestEnemy != null)
        {
            StartCoroutine(MoveTowardsEnemy(closestEnemy.transform));
        }
        else
        {
            isAttacking = false; // Reset attacking state if no enemy found
        }
    }

    private IEnumerator MoveTowardsEnemy(Transform enemy)
    {
        while (Vector2.Distance(transform.position, enemy.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemy.position, followSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Attack the enemy
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(1); // Deal damage (adjust the amount as needed)
        }

        // Optionally: Reset attacking state after reaching the enemy
        isAttacking = false; // Reset to allow orbiting again
    }
}