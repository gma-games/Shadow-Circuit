using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    public enum ObjectType
    {
        Eltunik, 
        Marad    
    }

    [Header("Mentési Beállítások")]
    public ObjectType tipus = ObjectType.Eltunik;
    public string uniqueId;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        if (UnityEditor.EditorUtility.IsPersistent(this)) return;

        if (string.IsNullOrEmpty(uniqueId))
        {
            GenerateId();
        }

        if (GetComponent<Switch>() != null)
        {
            tipus = ObjectType.Marad;
        }
        else if (GetComponent<Keycard>() != null || GetComponent<Key>() != null)
        {
            tipus = ObjectType.Eltunik;
        }
    }

    [ContextMenu("Új Egyedi ID Generálása!")]
    public void GenerateId()
    {
        uniqueId = System.Guid.NewGuid().ToString();
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    void Start()
    {
        if (string.IsNullOrEmpty(uniqueId)) return;

        if (PersistenceManager.Instance != null && PersistenceManager.Instance.IsObjectAlreadyCollected(uniqueId))
        {
            if (tipus == ObjectType.Eltunik)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void RegisterPickup()
    {
        if (!string.IsNullOrEmpty(uniqueId) && PersistenceManager.Instance != null)
        {
            PersistenceManager.Instance.RegisterInteraction(uniqueId);
        }
    }

    public void ResetState()
    {
        if (tipus == ObjectType.Eltunik)
        {
            gameObject.SetActive(true);

            Keycard keycard = GetComponent<Keycard>();
            if (keycard != null && keycard.door != null)
            {
                keycard.door.SetActive(true);
            }
        }
        else if (tipus == ObjectType.Marad)
        {
            Switch sw = GetComponent<Switch>();
            if (sw != null)
            {
                sw.Deactivate();
            }
        }
    }
}