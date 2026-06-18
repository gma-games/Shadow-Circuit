using UnityEngine;

public class Keycard : MonoBehaviour
{
    public GameObject door;

    private void Start()
    {
        SaveableObject saveable = GetComponent<SaveableObject>();
        if (saveable != null && PersistenceManager.Instance != null)
        {
            if (PersistenceManager.Instance.IsObjectAlreadyCollected(saveable.uniqueId))
            {
                if (door != null)
                {
                    door.SetActive(false);
                }

                if (UIManager.Instance != null)
                {
                    UIManager.Instance.SetKeycardUI(true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SaveableObject saveable = GetComponent<SaveableObject>();
            if (saveable != null)
            {
                saveable.RegisterPickup();
            }

            if (door != null)
            {
                door.SetActive(false);
            }

            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetKeycardUI(true);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound("Keycard");
            }

            gameObject.SetActive(false);
        }
    }
}