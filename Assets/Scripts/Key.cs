using UnityEngine;

public class Key : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // +1 hozzáadása
            GameManager.Instance.AddScore(1);

            AudioManager.Instance.PlaySound("Coin");

            // Objektum törlése
            Destroy(gameObject);
        }
    }
}