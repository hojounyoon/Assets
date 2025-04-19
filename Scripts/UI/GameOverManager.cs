using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI victoryText;
    public Button gameOverRestartButton;
    public Button victoryRestartButton;
    private EnemySpawner enemySpawner;

    void Start()
    {
        // Hide both screens initially
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        if (victoryScreen != null)
            victoryScreen.SetActive(false);
        
        // Find the EnemySpawner
        enemySpawner = FindObjectOfType<EnemySpawner>();
        
        // Set up the restart buttons
        if (gameOverRestartButton != null)
        {
            gameOverRestartButton.onClick.AddListener(RestartGame);
        }
        if (victoryRestartButton != null)
        {
            victoryRestartButton.onClick.AddListener(RestartFromVictory);
        }
        
        Debug.Log("GameOverManager initialized");
    }

    void Update()
    {
        // Show game over screen when player dies
        if (StatManager.Instance.isGameOver)
        {
            ShowGameOver();
        }
        // Show victory screen when all waves are completed
        else if (StatManager.Instance.isVictory)
        {
            ShowVictory();
        }
        else
        {
            // Hide both screens
            if (gameOverScreen != null)
                gameOverScreen.SetActive(false);
            if (victoryScreen != null)
                victoryScreen.SetActive(false);
        }
    }

    void ShowGameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            if (gameOverText != null)
            {
                gameOverText.text = "Game Over!";
            }
            GameManager.Instance.state = GameManager.GameState.GAMEOVER;
            Debug.Log("Game Over screen shown");
        }
        else
        {
            Debug.LogError("Game Over screen reference is missing!");
        }
    }

    void ShowVictory()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
            if (victoryText != null)
            {
                victoryText.text = "You Win!";
            }
            GameManager.Instance.state = GameManager.GameState.GAMEOVER;
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        
        // Reset the StatManager
        if (StatManager.Instance != null)
        {
            StatManager.Instance.ResetStats();
            StatManager.Instance.isGameOver = false;
            StatManager.Instance.isVictory = false;
        }
        
        // Reset game state
        GameManager.Instance.state = GameManager.GameState.PREGAME;
        
        // Hide game over and victory screens
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        if (victoryScreen != null)
            victoryScreen.SetActive(false);
            
        // Reset player health and position
        if (GameManager.Instance.player != null)
        {
            PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
            if (player != null)
            {
                // Reset health
                if (player.hp != null)
                {
                    player.hp.hp = 100; // Set to default max health
                }
                
                // Reset position to origin
                player.transform.position = Vector3.zero;
            }
        }

        // Destroy all existing enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        
        // Start the game
        GameManager.Instance.state = GameManager.GameState.COUNTDOWN;
    }

    public void RestartFromVictory()
    {
        Debug.Log("Restarting from victory...");
        
        // Reset the StatManager
        if (StatManager.Instance != null)
        {
            StatManager.Instance.ResetStats();
            StatManager.Instance.isGameOver = false;
            StatManager.Instance.isVictory = false;
            Debug.Log("StatManager reset complete");
        }
        
        // Reset game state
        GameManager.Instance.state = GameManager.GameState.PREGAME;
        Debug.Log("Game state set to PREGAME");
        
        // Hide game over and victory screens
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        if (victoryScreen != null)
            victoryScreen.SetActive(false);
        Debug.Log("Screens hidden");
            
        // Reset player health and position
        if (GameManager.Instance.player != null)
        {
            PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
            if (player != null)
            {
                // Reset health
                if (player.hp != null)
                {
                    player.hp.hp = 100; // Set to default max health
                    Debug.Log("Player health reset to 100");
                }
                
                // Reset position to origin
                player.transform.position = Vector3.zero;
                Debug.Log("Player position reset to origin");
            }
        }

        // Destroy all existing enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        Debug.Log($"Destroyed {enemies.Length} existing enemies");

        // Reset the wave count in EnemySpawner
        if (enemySpawner != null)
        {
            enemySpawner.ResetWaves();
            Debug.Log("EnemySpawner waves reset");
        }
        else
        {
            Debug.LogError("EnemySpawner not found!");
        }
        
        // Start the game
        GameManager.Instance.state = GameManager.GameState.COUNTDOWN;
        Debug.Log("Game state set to COUNTDOWN");
    }
} 