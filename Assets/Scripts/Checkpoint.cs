using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector2 savedPoisiton = Vector2.zero;

    [RuntimeInitializeOnLoadMethod]
    static void LoadCheckpointOnStartup()
    {
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            savedPoisiton = new Vector2(
                PlayerPrefs.GetFloat("CheckpointX"),
                PlayerPrefs.GetFloat("CheckpointY")
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            savedPoisiton = collision.transform.position;

            PlayerPrefs.SetFloat("CheckpointX", savedPoisiton.x);
            PlayerPrefs.SetFloat("CheckpointY", savedPoisiton.y);

            int currentScore = GameManager.Instance.score;
            PlayerPrefs.SetInt("CheckpointScore", currentScore);


            PlayerPrefs.Save(); 
        }
    }
}