using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [Header("Switch bveállítások")]
    public List<GameObject> linkedObjects;

    public bool isActive = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Activate()
    {
        isActive = true;
        spriteRenderer.color = Color.green; // Csak vizuális visszajelzés (később lehet sprite csere)

        // Végigmegyünk a listán, és minden ajtót kinyitunk
        foreach (GameObject obj in linkedObjects)
        {
            SwitchDoor door = obj.GetComponent<SwitchDoor>();
            if (door != null) door.Open();
        }
    }

    public void Deactivate()
    {
        isActive = false;
        spriteRenderer.color = Color.red;

        // Végigmegyünk a listán, és minden ajtót bezárunk
        foreach (GameObject obj in linkedObjects)
        {
            SwitchDoor door = obj.GetComponent<SwitchDoor>();
            if (door != null) door.Close();
        }
    }

    public void Toggle()
    {
        if (isActive) Deactivate();
        else Activate();

   
         AudioManager.Instance.PlaySound("Switch"); 
    }
}