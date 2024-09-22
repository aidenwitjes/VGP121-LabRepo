using UnityEngine;

public class KongState : MonoBehaviour
{
    public GameObject diddyKongPrefab; // Reference to the Diddy Kong prefab

    private bool singleState = true; // Initially, only one Kong is present
    private bool isDiddyKong = false; // Tracks if Diddy Kong is currently active
    private GameObject diddyKongInstance; // Reference to the Diddy Kong instance

    public void SpawnDiddyKong()
    {
        // Only spawn Diddy Kong if singleState is true (indicating there's only one Kong on screen)
        if (singleState)
        {
            // Instantiate Diddy Kong at the player's current position
            diddyKongInstance = Instantiate(diddyKongPrefab, transform.position, Quaternion.identity);

            // Set the collider of Diddy Kong to be a trigger
            Collider2D diddyCollider = diddyKongInstance.GetComponent<Collider2D>();
            if (diddyCollider != null)
            {
                diddyCollider.isTrigger = true;
            }

            singleState = false; // Set to false since we now have both Kongs on screen
            Debug.Log("Diddy Kong spawned! Now in Double Kong state.");
        }
        else
        {
            Debug.Log("Already in Double Kong state, Diddy Kong can't be spawned again.");
        }
    }
    public void SwapKongs()
    {
        if (!singleState) // Only allow swapping when both Kongs are present (i.e., not in single state)
        {
            isDiddyKong = !isDiddyKong; // Toggle between Donkey and Diddy

            if (isDiddyKong)
            {
                Debug.Log("Switched to Diddy Kong");
                // Set animations and behaviors for Diddy Kong here
            }
            else
            {
                Debug.Log("Switched to Donkey Kong");
                // Set animations and behaviors for Donkey Kong here
            }
        }
    }

    public bool IsDiddyKongActive()
    {
        return isDiddyKong;
    }

    public bool IsSingleState()
    {
        return singleState;
    }

    // Optional: Method to handle any specific actions when Diddy Kong is destroyed
    private void OnDestroy()
    {
        if (diddyKongInstance != null)
        {
            Destroy(diddyKongInstance);
        }
    }
}