using UnityEngine;

public enum FiringDirection
{
    Left,
    Right,
    Up,
    Down,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}

public class Clambo : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int numberOfProjectiles = 1; // Set in prefab
    [SerializeField] private Transform spawnPoint; // Single spawn point
    [SerializeField] private FiringDirection firingDirection; // Direction variable
    [SerializeField] private float projectileFireRate = 3f; // Time between shots
    private float timeSinceLastFire = 0f;

    public override void Start()
    {
        base.Start();
        if (projectileFireRate <= 0)
            projectileFireRate = 3f;
    }

    private void Update()
    {
        PlayerController pc = GameManager.Instance.PlayerInstance;

        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        float distance = Vector2.Distance(pc.transform.position, transform.position);

        if (curPlayingClips[0].clip.name.Contains("Clambo_Idle"))
        {
            if (Time.time >= timeSinceLastFire + projectileFireRate && distance <= 10.0)
            {
                anim.SetTrigger("Fire");
                timeSinceLastFire = Time.time;
            }
        }
    }

    public void Fire()
    {
        Vector2 direction = Vector2.zero;

        // Directly set the direction based on the inspector value
        switch (firingDirection)
        {
            case FiringDirection.Left: direction = Vector2.left; break;
            case FiringDirection.Right: direction = Vector2.right; break;
            case FiringDirection.Up: direction = Vector2.up; break;
            case FiringDirection.Down: direction = Vector2.down; break;
            case FiringDirection.UpLeft: direction = new Vector2(-1, 1).normalized; break;
            case FiringDirection.UpRight: direction = new Vector2(1, 1).normalized; break;
            case FiringDirection.DownLeft: direction = new Vector2(-1, -1).normalized; break;
            case FiringDirection.DownRight: direction = new Vector2(1, -1).normalized; break;
        }

        switch (numberOfProjectiles)
        {
            case 1:
                FireProjectile(direction); //Single shot LRUD
                break;
            case 2:
                FireTwoProjectiles(direction); //Double shot "V" pattern either LRUD
                break;
            case 3:
                FireThreeProjectiles(direction); //Triple shot in the direction LRUD and then perpendicular directions "_|_"
                break;
            case 5:
                FireFiveProjectiles(direction); //Penta shot in a fan pattern "\\|//" either LRUD
                break;
            default:
                Debug.LogWarning("Unsupported number of projectiles");
                break;
        }
    }

    private void FireProjectile(Vector2 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetVelocity(direction.x, direction.y);
    }

    private void FireTwoProjectiles(Vector2 direction)
    {
        FireProjectile(Quaternion.Euler(0, 0, -30) * direction);
        FireProjectile(Quaternion.Euler(0, 0, 30) * direction);
    }

    private void FireThreeProjectiles(Vector2 direction)
    {
        FireProjectile(direction);
        FireProjectile(Quaternion.Euler(0, 0, -90) * direction);
        FireProjectile(Quaternion.Euler(0, 0, 90) * direction);
    }

    private void FireFiveProjectiles(Vector2 direction)
    {
        FireProjectile(direction);
        FireProjectile(Quaternion.Euler(0, 0, -60) * direction);
        FireProjectile(Quaternion.Euler(0, 0, -30) * direction); 
        FireProjectile(Quaternion.Euler(0, 0, 30) * direction);  
        FireProjectile(Quaternion.Euler(0, 0, 60) * direction);
    }
}