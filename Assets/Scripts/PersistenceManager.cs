using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance { get; private set; }

    // Elmentett idk a checkpoint előtről
    private HashSet<string> permanentSavedIds = new HashSet<string>();

    // a legutóbbi checkpoint után felvett id-k
    private HashSet<string> currentRunIds = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadPermanentData(); // permanentsave data betöltés
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Az érme hívja amior felveszük
    public void RegisterInteraction(string uniqueId)
    {
        // Csak akkor rakjuk a currentbe ha nincs  a permanentbe
        if (!permanentSavedIds.Contains(uniqueId))
        {
            currentRunIds.Add(uniqueId);
            Debug.Log("Currentbe került: " + uniqueId);
        }
    }

    // Véglegesen fel lett véve a táérgy? 
    public bool IsObjectAlreadyCollected(string uniqueId)
    {
        // permantentet check
        return permanentSavedIds.Contains(uniqueId);
    }

    // Mindent a currentből a permanentbe
    public void CommitCurrentRun()
    {
        foreach (string id in currentRunIds)
        {
            permanentSavedIds.Add(id);
        }
        currentRunIds.Clear(); 
        SavePermanentData();
        Debug.Log("Checkpoint: Permanent saved");
    }

    // current ürítés
    public void ResetCurrentRun()
    {
        SaveableObject[] allObjects = Object.FindObjectsByType<SaveableObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (SaveableObject obj in allObjects)
        {
            if (currentRunIds.Contains(obj.uniqueId))
            {
                obj.ResetState();
            }
        }

        currentRunIds.Clear();
        Debug.Log("Current kiürítve");
    }


    private void SavePermanentData()
    {
        string allIds = string.Join(",", permanentSavedIds);
        PlayerPrefs.SetString("SavedObjectIds", allIds);
        PlayerPrefs.Save();
    }

    private void LoadPermanentData()
    {
        if (PlayerPrefs.HasKey("SavedObjectIds"))
        {
            string savedData = PlayerPrefs.GetString("SavedObjectIds");
            string[] ids = savedData.Split(',');

            foreach (string id in ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    permanentSavedIds.Add(id);
                }
            }
        }

    }

    public void ClearAllSavedData()
    {
        permanentSavedIds.Clear();
        currentRunIds.Clear();
        PlayerPrefs.DeleteKey("SavedObjectIds");
        PlayerPrefs.Save();
        Debug.Log("Mentett tárgyak listája teljesen kiürítve az új játékhoz!");
    }

}