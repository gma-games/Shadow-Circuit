using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD Elements")]
    public TextMeshProUGUI scoreText;
    public Image healthBar;
    public UnityEngine.UI.Image healthBarFill;

    public Image keycardIcon;

    [Header("Menus & Screens")]
    public GameObject pauseMenu;
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

        // Induláskor halványítjuk a Keycard ikont
        SetKeycardUI(false);
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

    public void SetKeycardUI(bool hasKeycard)
    {
        if (keycardIcon != null)
        {
            // Lekérjük az ikon jelenlegi színét
            Color iconColor = keycardIcon.color;

            // Ha megvan a kártya, az Alpha (átlátszóság) 1f (100%), ha nincs, akkor 0.3f (30% halvány)
            iconColor.a = hasKeycard ? 1f : 0.3f;

            // Visszaadjuk az új színt az ikonnak
            keycardIcon.color = iconColor;
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