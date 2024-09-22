using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
public class Pickup : MonoBehaviour
{
    public GameObject swordfishPrefab;
    public Animator animalCrateEnguarde;

    public enum PickupType
    {
        Banana,
        BananaBunch,
        DKBarrel,
        CheckPointBarrel,
        AnimalCrate,
       
    }

    [SerializeField] private PickupType type;

    public AudioClip pickupSound;

    SpriteRenderer sr;
    AudioSource audioSource;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = GameManager.Instance.SFXGroup;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Collider2D myCollider = GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(myCollider, collider);

            switch (type)
            {
                case PickupType.Banana:
                    GameManager.Instance.score++;
                    break;
                case PickupType.BananaBunch:
                    GameManager.Instance.score += 10;
                    break;
                case PickupType.DKBarrel:
                    GameManager.Instance.lives++;
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
                    // Trigger the crate animation
                    animalCrateEnguarde.SetTrigger("Open"); // Replace with your actual trigger name

                    // Spawn Enguarde after the animation ends
                    SpawnEnguarde();

                    // Optionally destroy the pickup after a delay
                    Destroy(gameObject, 0.5f); // Adjust the delay as necessary
                    break;
            }
            sr.enabled = false;
            audioSource.PlayOneShot(pickupSound);

            Destroy(gameObject, pickupSound.length);
        }
    }
    private void SpawnEnguarde()
    {
        // Spawn the swordfish at the crate's position
        Instantiate(swordfishPrefab, transform.position, Quaternion.identity);

        // Optionally, add any additional logic for the spawning (like sound effects)
        Debug.Log("Enguarde spawned!");
    }
}
