using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongSwitch : MonoBehaviour
{
    [SerializeField] private GameObject donkeyKong;  // Donkey Kong GameObject
    [SerializeField] private GameObject diddyKong;   // Diddy Kong GameObject
    [SerializeField] private CameraFollow cameraFollow; // Reference to your camera follow script

    private GameObject activeKong;   // The Kong currently controlled by the player
    private GameObject inactiveKong; // The Kong following with a delay
    private bool isSwitching = false;
    [SerializeField] private float followDelay = 0.5f;  // Delay time for the inactive Kong to follow
    [SerializeField] private Vector3 inactiveZOffset = new Vector3(0, 0, 1); // Offset for z-axis positioning

    private Queue<Vector3> movementHistory;  // To store the positions for delayed following

    void Start()
    {
        // Set initial active Kong
        activeKong = donkeyKong;
        inactiveKong = diddyKong;

        // Initialize movement history
        movementHistory = new Queue<Vector3>();

        // Disable Diddy Kong's PlayerController at the start
        SetPlayerControllerState(diddyKong, false);
        SetColliderState(diddyKong, false);
    }

    void Update()
    {
        // Switch Kongs when the player presses the switch button
        if (Input.GetButtonDown("Fire1") && !isSwitching)
        {
            StartCoroutine(SwitchKongs());
        }

        // Record the active Kong's movement for delayed following
        movementHistory.Enqueue(activeKong.transform.position);

        // Remove excess history to maintain the delay
        if (movementHistory.Count > Mathf.Round(followDelay / Time.deltaTime))
        {
            movementHistory.Dequeue();
        }

        // Make the inactive Kong follow the delayed positions
        if (movementHistory.Count > 0)
        {
            inactiveKong.transform.position = movementHistory.Peek() + inactiveZOffset;
        }
    }

    IEnumerator SwitchKongs()
    {
        isSwitching = true;

        // Swap the active and inactive Kongs
        GameObject temp = activeKong;
        activeKong = inactiveKong;
        inactiveKong = temp;

        // Enable the PlayerController on the active Kong and disable it on the inactive one
        SetPlayerControllerState(activeKong, true);
        SetPlayerControllerState(inactiveKong, false);

        // Make inactive Kong non-interactable
        SetColliderState(inactiveKong, false);
        SetColliderState(activeKong, true);

        // Update the camera target to the new active Kong
        cameraFollow.SetTarget(activeKong.transform);
        Debug.Log($"Switched to {activeKong.name}. Updated camera target.");

        // Clear movement history to prevent instant jumps after the switch
        movementHistory.Clear();

        yield return new WaitForSeconds(0.1f);  // Optional: Short delay before allowing another switch

        isSwitching = false;
    }

    private void SetPlayerControllerState(GameObject kong, bool state)
    {
        PlayerController controller = kong.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = state; // Enable or disable the controller
        }
    }

    private void SetColliderState(GameObject kong, bool state)
    {
        Collider2D collider = kong.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = state; // Enable or disable the collider
        }
    }
}