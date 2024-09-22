using UnityEngine;

public class Croctopus : Enemy
{
    [SerializeField] private float speed = 2f; // Speed of the Croctopus
    [SerializeField] private Vector2 startPoint; // Starting point of the rectangle
    [SerializeField] private Vector2 size; // Width and height of the rectangle
    [SerializeField] private float distanceThreshold = 0.1f; // Adjustable distance threshold for reaching target
    [SerializeField] private bool isClockwise = true; // New boolean to control clockwise/counterclockwise movement

    private Vector2 targetPosition;
    private int currentDirection = 0; // 0 = right, 1 = up, 2 = left, 3 = down

    public override void Start()
    {
        base.Start(); // Call base class Start() for enemy initialization

        if (size == Vector2.zero)
            size = new Vector2(5f, 3f); // Default size, can adjust as needed

        if (startPoint == Vector2.zero)
            startPoint = transform.position; // Default to current position

        // Set the initial target position (assuming starting direction is right)
        targetPosition = startPoint + new Vector2(size.x, 0);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // Move towards the target position in a straight line
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the Croctopus has reached the target position
        if (Vector2.Distance(transform.position, targetPosition) < distanceThreshold)
        {
            // Change direction based on the current direction and movement type (clockwise or counterclockwise)
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        if (isClockwise)
        {
            // Clockwise direction: right -> up -> left -> down
            currentDirection++;
            if (currentDirection > 3)
                currentDirection = 0;
        }
        else
        {
            // Counterclockwise direction: right -> down -> left -> up
            currentDirection--;
            if (currentDirection < 0)
                currentDirection = 3;
        }

        // Set the new target position based on the current direction
        switch (currentDirection)
        {
            case 0: // Move right
                targetPosition = startPoint + new Vector2(size.x, 0);
                break;
            case 1: // Move up
                targetPosition = startPoint + new Vector2(size.x, size.y);
                break;
            case 2: // Move left
                targetPosition = startPoint + new Vector2(0, size.y);
                break;
            case 3: // Move down
                targetPosition = startPoint;
                break;
        }
    }
}