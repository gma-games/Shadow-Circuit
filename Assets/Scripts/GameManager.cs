using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public int currentLevel = 1;
    public int playerLives = 3; // 3 élete van alapból az újrakezdés előtt
    public int score = 0;       // Ez veszi át a korábbi "key" szerepét
    public bool isPaused = false;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {

        playerLives = PlayerPrefs.GetInt("PlayerLives", 3);
        StartGame();
    }

    private void Update()
    {
        // Figyeli a pause gombot 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        isPaused = false;
       
        score = PlayerPrefs.GetInt("CheckpointScore", 0);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreDisplay(score);
        }

    }

    public void PlayerDied(PlayerHealth player)
    {
        playerLives--; // Levonunk egy életet
        Debug.Log("Élet elvesztve! Maradt: " + playerLives);

        PlayerPrefs.SetInt("PlayerLives", playerLives);
        PlayerPrefs.Save();

        if (playerLives <= 0)
        {
            // Ha elfogyott az összes élet, visszaállítjuk 3-ra a következő játékhoz
            PlayerPrefs.SetInt("PlayerLives", 3);
            UIManager.Instance.ShowGameOverScreen();
            Time.timeScale = 0; // Megállítjuk az időt és a fizikát
        }
        else
        {
            // Még van élet, újraéledés
            player.Respawn();
        }
    }

    public void LoadLevel(int levelNumber)
    {
        
        PlayerPrefs.SetInt("LevelReached", levelNumber);

        // Fontos: a Unity Build Settings-ben lévő pálya neveidet kell majd használni (pl. "Level_" + levelNumber)
        // Egyelőre betöltjük a string alapján, ha kell.
        SceneManager.LoadScene(levelNumber);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; // Megállítja a fizikát és a játékidőt

        UIManager.Instance.ShowPauseMenu(true);
        Debug.Log("Játék megállítva.");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // Újraindítja a játékidőt

        UIManager.Instance.ShowPauseMenu(false);
        Debug.Log("Játék folytatódik.");
    }

    // pontok/kulcsok felvétele
    public void AddScore(int amount)
    {
        score += amount;
        UIManager.Instance.UpdateScoreDisplay(score);
    }
}