using System.Collections.Generic;
using UnityEngine;

public class PuzzleElement : MonoBehaviour
{
    // A diagramod alapján az állapotok:
    public enum PuzzleState { Inactive, Active, Completed }
    public PuzzleState puzzleState = PuzzleState.Inactive;

    [Header("Kapcsolók/gombok/")]
    public List<Switch> requiredSwitches; 

    [Header("Ajtók vagy platformok")]
    public List<GameObject> objectsToTrigger; 

    void Update()
    {
        // Ha még nincs megoldva a puzzle, folyamatosan ellenőrizzük az állapotát
        if (puzzleState != PuzzleState.Completed)
        {
            CheckCompletion();
        }
    }

    public void CheckCompletion()
    {
        if (requiredSwitches.Count == 0) return;

        bool allSwitchesActive = true;

        // Végignézzük az összes elemet
        foreach (Switch sw in requiredSwitches)
        {
            if (!sw.isActive)
            {
                allSwitchesActive = false; // Ha egy mondjuk nincs benyomba akkro nem jó
                break;
            }
        }

        // Ha mind be van nyomva, akkor kész
        if (allSwitchesActive)
        {
            puzzleState = PuzzleState.Completed;
            TriggerEffect();
        }
    }

    private void TriggerEffect()
    {
        Debug.Log("Puzzle sikeresen megoldva!");

       
        foreach (GameObject obj in objectsToTrigger)
        {
            // Ha ajtó van a listában, kinyitjuk
            SwitchDoor door = obj.GetComponent<SwitchDoor>();
            if (door != null) door.Open();

            // Ha mozgó platform van a listában, elindítjuk
            MovingPlatform platform = obj.GetComponent<MovingPlatform>();
            if (platform != null) platform.isMoving = true;
        }

      
        // AudioManager.Instance.PlaySound("PuzzleSolved");
    }
}