using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public void ActivateCheckpoint()
    {
        GameManager.Instance.UpdateCheckpoint(transform);
        Debug.Log("Checkpoint updated!");
    }
}