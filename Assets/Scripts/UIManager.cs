using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD Elements")]
    public TextMeshProUGUI scoreText; // A régi "coinText" megfelelője
    public Image healthBar;           // A régi "healthImage" megfelelője
    public UnityEngine.UI.Image healthBarFill;

    [Header("Menus & Screens")]
    public GameObject pauseMenu;      // A régi "container" a PauseMenu.cs-ből
    public GameObject gameOverScreen;
    public GameObject winScreen;     

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
        // Biztosítjuk, hogy induláskor a menük el legyenek rejtve
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        if (winScreen != null) winScreen.SetActive(false);

        // Alapértékek beállítása
        UpdateScoreDisplay(0);
    }

    //  KÉPERNYŐ FRISSÍTŐ METÓDUSOK 

    public void UpdateScoreDisplay(int currentScore)
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }

    public void UpdateHealthDisplay(float healthPercentage)
    {
        // Ide egy 0.0 és 1.0 közötti számot várunk (pl. 0.5 = 50%)
        if (healthBar != null)
        {
            healthBar.fillAmount = healthPercentage;
        }
    }

    public void ShowPauseMenu(bool isPaused)
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
        }
    }

    public void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void ShowWinScreen()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }
    }

    //  GOMBOKHOZ RENDELHETŐ FUNKCIÓK

    public void ResumeButton()
    {
        GameManager.Instance.ResumeGame();
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu"); 
    }
}