using UnityEngine;

public class Victory : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GameManager.Instance.TriggerVictory();
        }
    }
}