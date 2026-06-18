using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SaveableObjectFixer
{
    [MenuItem("Tools/Összes ID egyedivé tétele")]
    public static void FixAllIDsInScene()
    {
        SaveableObject[] allObjects = Object.FindObjectsByType<SaveableObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        HashSet<string> usedIds = new HashSet<string>();
        int fixedCount = 0;

        foreach (SaveableObject obj in allObjects)
        {
            bool needsNewId = false;

            if (string.IsNullOrEmpty(obj.uniqueId))
            {
                needsNewId = true;
            }
            else if (usedIds.Contains(obj.uniqueId))
            {
                needsNewId = true;
            }

            if (needsNewId)
            {
                obj.GenerateId();
                EditorUtility.SetDirty(obj);
                fixedCount++;
            }

            usedIds.Add(obj.uniqueId);
        }

        Debug.Log($"<color=green>{fixedCount} db hibás vagy másolt ID javítva!</color>");
    }
}