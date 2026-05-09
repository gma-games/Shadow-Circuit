using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Manual Interaction Settings")]
    public float interactRange = 1.5f;
    // Interaktáló tárgyak listája , amik a közelben vannak (kapcsolók, ajtók, stb.)
    public List<GameObject> nearbyObjects = new List<GameObject>();

    void Update()
    {
        // E betű megnyomására interakcióba lép 
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (nearbyObjects.Count > 0)
        {
            
            GameObject objToInteract = nearbyObjects[0];
            

            Switch toggleSwitch = objToInteract.GetComponent<Switch>();
            if (toggleSwitch != null)
            {
                toggleSwitch.Toggle();
            }
        }
    }

    //  AUTOMATIKUS INTERAKCIÓK (Kulcsok, Célvonal, Checkpointok) 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Kézi interakcióhoz (kapcsolókhoz) hozzáadjuk a listához, ha a közelébe érünk
        if (collision.CompareTag("Interactable"))
        {
            if (!nearbyObjects.Contains(collision.gameObject))
            {
                nearbyObjects.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Kézi interakcióhoz eltávolítjuk a listából, ha elsétálunk tőle
        if (collision.CompareTag("Interactable"))
        {
            if (nearbyObjects.Contains(collision.gameObject))
            {
                nearbyObjects.Remove(collision.gameObject);
            }
        }
    }
}