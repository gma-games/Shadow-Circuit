using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [Header("Utolsó Pálya Beállításai")]
    public bool isFinalLevel = false;
    public string creditsSceneName = "Credits"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Checkpoint.ResetCheckpoint();

            if (isFinalLevel)
            {
                SceneManager.LoadScene(creditsSceneName);
            }
            else
            {
                Time.timeScale = 0;
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowWinScreen();
                }
            }
        }
    }
}