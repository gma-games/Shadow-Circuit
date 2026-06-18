using UnityEngine;

public class Key : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // +1 hozzáadása
            GameManager.Instance.AddScore(1);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound("Key");
            }

            SaveableObject saveable = GetComponent<SaveableObject>();
            if (saveable != null)
            {
                saveable.RegisterPickup();
            }

            // Objektum törlése
            gameObject.SetActive(false);
        }
    }
}