using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [Header("Sztori Szövegbuborék")]
    public GameObject speechBubble;
    public TextMeshProUGUI bubbleTextDisplay;
    [TextArea(3, 5)]
    public string storyText;

    [Header("Egyedi Beállítások")]
    public bool showAtGameStart = false;

    private bool isActivated = false;

    private void Start()
    {
        if (bubbleTextDisplay != null)
        {
            bubbleTextDisplay.text = storyText;
        }

        if (speechBubble != null)
        {
            speechBubble.SetActive(showAtGameStart);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (speechBubble != null && !string.IsNullOrEmpty(storyText))
            {
                speechBubble.SetActive(true);
            }

            if (!isActivated)
            {
                isActivated = true;

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySound("Checkpoint");
                }

                string currentScene = SceneManager.GetActiveScene().name;

                PlayerPrefs.SetFloat("CheckpointX_" + currentScene, collision.transform.position.x);
                PlayerPrefs.SetFloat("CheckpointY_" + currentScene, collision.transform.position.y);

                PlayerPrefs.SetInt("CheckpointScore_" + currentScene, GameManager.Instance.score);
                PlayerPrefs.Save();

                if (PersistenceManager.Instance != null)
                {
                    PersistenceManager.Instance.CommitCurrentRun();
                }

                Debug.Log($"Mentés sikeres ezen a pályán: {currentScene}");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !showAtGameStart)
        {
            if (speechBubble != null)
            {
                speechBubble.SetActive(false);
            }
        }
    }

    public static Vector2 GetSavedPositionForCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (PlayerPrefs.HasKey("CheckpointX_" + currentScene))
        {
            return new Vector2(
                PlayerPrefs.GetFloat("CheckpointX_" + currentScene),
                PlayerPrefs.GetFloat("CheckpointY_" + currentScene)
            );
        }

        return Vector2.zero;
    }

    public static void ResetCheckpoint()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.DeleteKey("CheckpointX_" + currentScene);
        PlayerPrefs.DeleteKey("CheckpointY_" + currentScene);
        PlayerPrefs.DeleteKey("CheckpointScore_" + currentScene);
        PlayerPrefs.Save();
    }
}