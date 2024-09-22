using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public Action<int> OnLifeValueChanged;
    public Action<int> OnScoreValueChanged;
    //Public UnityEvent<int> OnLifeValueChanged;

    //Private Lives variable (_ to indicate an internal variable)
    private int _lives = 3;

    //Variable to track the pause state of the game
    private bool isPaused = false;

    private float deathTimer;
    private bool isPlayerDead = false;

    //Public variable for getting and setting lives
    public int lives
    {
        get
        {
            return _lives;
        }
        set
        {
            //All lives lost (zero counts as a life due to the check
            if (value < 0)
            {
                GameOver();
                return;
            }

            //Lost a life
            if (value < _lives)
            {
                HandlePlayerDeath();
            }

            //Cannot roll over the maximum amount of lives
            if (value >= maxLives)
            {
                value = maxLives;
            }
            _lives = value;
            OnLifeValueChanged?.Invoke(_lives);

            Debug.Log($"Lives value on {gameObject.name} has changed to {lives}");
        }
    }

    //Private Score variable (_ to indicate an internal variable)
    private int _score = 0;

    //Public variable for getting and setting score
    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            //Cannot roll over the maximum score
            if (value == maxScore)
            {
                value = 0;
                //Update lives by 1
            }
            _score = value;
            OnScoreValueChanged?.Invoke(_score);

            Debug.Log($"Score value on {gameObject.name} has changed to {score}");
        }
    }

    public AudioMixerGroup SFXGroup;
    public AudioMixerGroup MusicGroup;

    //Max lives possible
    [SerializeField] private int maxLives = 99;
    //Max score possible
    [SerializeField] private int maxScore = 100;
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private AudioClip pauseClip;
    [SerializeField] private AudioClip victoryClip;
    [SerializeField] private AudioClip deathClip;

    AudioSource audioSource;
    [HideInInspector] public PlayerController PlayerInstance => playerInstance;
    private PlayerController playerInstance;
    private Transform currentCheckpoint;
    private MenuController currentMenuController;
    private void Awake()
    {
        //If we are the first instance of the gamemanager object - ensure that our instance variable is filled and we cannot be destroyed when loading new levels.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return; //Early exit out of the function
        }

        //If we are down here in execution - that means that the above if statement didn't run - which means we are a clone
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        audioSource = GetComponent<AudioSource>();

        if (!currentMenuController) return;

        if (isPlayerDead)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer >= 2f) // Adjust based on your animation length
            {
                Respawn();
                isPlayerDead = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && (SceneManager.GetActiveScene().name == "Level 1"))
        {
            if (isPaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
            audioSource.PlayOneShot(pauseClip);
        }
    }

    public void PauseGame()
    {
        currentMenuController.SetActiveState(MenuController.MenuStates.Pause);
        Time.timeScale = 0;
        isPaused = true;
        Debug.Log("Game Paused");
    }

    public void UnpauseGame()
    {
        currentMenuController.JumpBack();
        Time.timeScale = 1;
        isPaused = false;
        Debug.Log("Game Unpaused");
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void GameOver()
    {
        if ((SceneManager.GetActiveScene().name == "Level 1"))
        {
            SceneManager.LoadScene("GameOver");
            Debug.Log("Out of lives. Game Over!");
        }
    }

    public void TriggerVictory()
    {
        // Logic to handle victory, like updating score or lives if needed
        // Then call the victory animation on the player
        PlayerController player = PlayerInstance; // Assuming you have a reference to the player
        if (player != null)
        {
            Animator animator = player.GetComponent<Animator>();
            if (animator != null)
            {
                audioSource.PlayOneShot(victoryClip);
                animator.SetTrigger("Victory");
            }

            // Start the scene change after a delay
            StartCoroutine(ChangeSceneAfterDelay("VictoryScreen", 2f)); // Adjust delay as needed
        }
    }

    private IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    private void HandlePlayerDeath()
    {
        audioSource.PlayOneShot(deathClip);
        PlayerController player = PlayerInstance; // Assuming you have a reference to the player
        if (player != null)
        {
            Animator animator = player.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Death");
            }

            // Disable the player's collider here if needed
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                playerCollider.enabled = false; // Disable the player's collider
            }

            isPlayerDead = true; // Set the death flag
            deathTimer = 0f; // Reset the timer
        }
    }

    void Respawn()
{
    if (currentCheckpoint != null)
    {
        playerInstance.transform.position = currentCheckpoint.position;
    }
    else
    {
        Debug.LogWarning("Attempted to respawn, but currentCheckpoint is null.");
        // Optionally, handle the case when there's no checkpoint set
    }
        Collider2D playerCollider = playerInstance.GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }
}

    public void SpawnPlayer(Transform spawnLocation)
    {
        playerInstance = Instantiate(playerPrefab, spawnLocation.position, Quaternion.identity);
        currentCheckpoint = spawnLocation;
    }

    public void UpdateCheckpoint(Transform updatedCheckpoint)
    {
        currentCheckpoint = updatedCheckpoint;
        Debug.Log($"Checkpoint updated to position: {currentCheckpoint.position}");
    }

    public void SetMenuController(MenuController menuController)
    {
        currentMenuController = menuController;
    }
}
