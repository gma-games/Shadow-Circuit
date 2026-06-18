using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Switch : MonoBehaviour
{
    [Header("Switch beállítások")]
    public List<GameObject> linkedObjects;
    public bool isActive = false;
    private SpriteRenderer spriteRenderer;

    [Header("UI és Interakció")]
    public GameObject speechBubble;
    public TextMeshProUGUI bubbleTextDisplay;
    [TextArea(1, 3)]
    public string interactText = "Az 'E' gombbal használható";

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (bubbleTextDisplay != null) bubbleTextDisplay.text = interactText;
        if (speechBubble != null) speechBubble.SetActive(false);

        SaveableObject saveObj = GetComponent<SaveableObject>();
        if (saveObj != null && PersistenceManager.Instance != null)
        {
            if (PersistenceManager.Instance.IsObjectAlreadyCollected(saveObj.uniqueId))
            {
                Activate();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (speechBubble != null) speechBubble.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (speechBubble != null) speechBubble.SetActive(false);
        }
    }

    public void Activate()
    {
        isActive = true;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.green;

        foreach (GameObject obj in linkedObjects)
        {
            if (obj != null)
            {
                SwitchDoor door = obj.GetComponent<SwitchDoor>();
                if (door != null) door.Open();
            }
        }

        SaveableObject saveObj = GetComponent<SaveableObject>();
        if (saveObj != null)
        {
            saveObj.RegisterPickup();
        }
    }

    public void Deactivate()
    {
        isActive = false;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.red;

        foreach (GameObject obj in linkedObjects)
        {
            if (obj != null)
            {
                SwitchDoor door = obj.GetComponent<SwitchDoor>();
                if (door != null) door.Close();
            }
        }
    }

    public void Toggle()
    {
        if (isActive) Deactivate();
        else Activate();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Switch");
        }
    }
}