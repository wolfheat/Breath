
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ItemID
{
    public string Value;
}

public abstract class Item : Interactable
{

    [SerializeField] private ItemID id;
    public string ID => id.Value;

    [ContextMenu("Force reset ID")]
    private void ResetId()
    {
        id.Value = Guid.NewGuid().ToString();
        Debug.Log("Setting new ID on object: " + gameObject.name+ gameObject+" id:"+id.Value);
    }   

    //Need to check for duplicates when copying a gameobject/component
    public static bool IsUnique(string ID)
    {
        return Resources.FindObjectsOfTypeAll<Item>().Count(x => x.ID == ID) == 1;
    }
    public static List<string> GitItemsWithID(string ID)
    {
        return (List<string>)Resources.FindObjectsOfTypeAll<Item>().Where(x => x.ID == ID).Select(x => x.name).ToList();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        if (!Application.isEditor || Application.isPlaying)
        {
            return;
        }
        // Check if ID is empty (not set during instantiation)
        if (string.IsNullOrEmpty(id.Value) || !IsUnique(id.Value))
        {

            if (!string.IsNullOrEmpty(id.Value))
            {
                Debug.Log("Several Items have this ID: " + id.Value);
                //foreach(var s in GitItemsWithID(id.Value))
                //{
                //    Debug.Log("Items holding this ID: " + s);
                //}
            }
            Debug.Log("Setting new ID on object: " + gameObject.name + gameObject + " id:" + id.Value);

            ResetId();
        }
    }
#endif 

}
