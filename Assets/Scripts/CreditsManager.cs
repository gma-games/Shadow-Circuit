using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [Header("Görgetés Beállításai")]
    public RectTransform textToScroll; 
    public float scrollSpeed = 50f;
    public float stopPositionY = 2000f; 

    [Header("Vissza a Menübe")]
    public string mainMenuSceneName = "Main Menu"; 

    void Update()
    {
        if (textToScroll != null && textToScroll.anchoredPosition.y < stopPositionY)
        {
            textToScroll.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            ReturnToMenu();
        }
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}