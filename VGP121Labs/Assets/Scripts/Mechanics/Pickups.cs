using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        Banana,
        BananaBunch,
        DKBarrel,
        CheckPointBarrel,
        AnimalCrate,
       
    }

    [SerializeField] private PickupType type;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerController pc = collider.GetComponent<PlayerController>();

            switch (type)
            {
                case PickupType.Banana:
                    GameManager.Instance.score++;
                    break;
                case PickupType.BananaBunch:
                    GameManager.Instance.score += 10;
                    break;
                case PickupType.DKBarrel:
                    break;
                case PickupType.CheckPointBarrel:
                    Checkpoint checkpoint = GetComponent<Checkpoint>();
                    if (checkpoint != null)
                    {
                        checkpoint.ActivateCheckpoint();
                    }
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    return;
                case PickupType.AnimalCrate:
                    break;
            }
            Destroy(gameObject);
        }
    }
}
