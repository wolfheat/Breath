using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public static ItemCreator Instance;
    [SerializeField] PickableItem[] pickablePrefabs;
    [SerializeField] DestructableItem[] destructablePrefabs;
    [SerializeField] GameObject itemHolder;
    [SerializeField] Player player;
    [SerializeField] GameObject[] destructablesHolders;
    [SerializeField] GameObject[] pickablesHolders;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void SetGameData()
    {
        // Save all items to file

        //Destructables
        SaveItem[][] destructableSave = new SaveItem[destructablesHolders.Length][];
        for (int i = 0; i < destructablesHolders.Length; i++)
        {
            GameObject holder = destructablesHolders[i];
            // Get All Destructable children
            DestructableItem[] items = holder.GetComponentsInChildren<DestructableItem>();
            Debug.Log("Found "+items.Length+" items in holder "+holder.name);
            List<SaveItem> saveItems = new List<SaveItem>();
            foreach (var item in items)
            {
                if (item.gameObject.activeSelf)
                    saveItems.Add(new SaveItem() { id = GetItemID(item.Data as DestructableData), position = SavingUtility.Vector3AsV3(item.transform.position), rotation = SavingUtility.Vector3AsV3(item.transform.forward) });
            }
            destructableSave[i] = saveItems.ToArray();
        }
        SavingUtility.playerGameData.Destructables = destructableSave;
        
        //Pickables
        SaveItem[][] pickableSave = new SaveItem[pickablesHolders.Length][];
        for (int i = 0; i < pickablesHolders.Length; i++)
        {
            GameObject holder = pickablesHolders[i];
            // Get All Destructable children
            PickableItem[] items = holder.GetComponentsInChildren<PickableItem>();
            Debug.Log("Found "+items.Length+" items in holder "+holder.name);
            List<SaveItem> saveItems = new List<SaveItem>();
            foreach (var item in items)
            {
                if (item.gameObject.activeSelf)
                    saveItems.Add(new SaveItem() { id = GetItemID(item.Data as ResourceData), position = SavingUtility.Vector3AsV3(item.transform.position), rotation = SavingUtility.Vector3AsV3(item.transform.forward) });
            }
            pickableSave[i] = saveItems.ToArray();
        }
        SavingUtility.playerGameData.Pickables = pickableSave;

    }

    private int GetItemID(DestructableData item)
    {
        return (int)item.boxType;
    }
    private int GetItemID(ResourceData item)
    {
        return (int)item.resource;
    }

    public void LoadFromFile()
    {
        // Clear all old data
        foreach (var holder in destructablesHolders)
            foreach (Transform item in holder.transform)
                Destroy(item.gameObject);
        foreach (var holder in pickablesHolders)
            foreach (Transform item in holder.transform)
                Destroy(item.gameObject);

        Debug.Log("All items destroyed");
        SaveItem[][] destructableLoad = SavingUtility.playerGameData.Destructables;
        for (int i = 0; i < destructableLoad.Length; i++)
        {
            foreach(var item in destructableLoad[i])
            {
                Quaternion rotation = Quaternion.LookRotation(SavingUtility.V3AsVector3(item.rotation));
                Instantiate(destructablePrefabs[item.id], SavingUtility.V3AsVector3(item.position),rotation, pickablesHolders[i].transform);
            }
        }

        SaveItem[][] pickableLoad = SavingUtility.playerGameData.Pickables;
        for (int i = 0; i < pickableLoad.Length; i++)
        {
            foreach(var item in pickableLoad[i])
            {
                Quaternion rotation = Quaternion.LookRotation(SavingUtility.V3AsVector3(item.rotation));
                Instantiate(pickablePrefabs[item.id], SavingUtility.V3AsVector3(item.position),rotation, pickablesHolders[i].transform);
            }
        }




    }


    public void InstantiateResourceAt(Resource type, Vector3 pos, Vector3 forward)
    {
        InstantiateResourceAt((int) type, pos, forward);
    }
    public void InstantiateResourceAt(int type, Vector3 pos, Vector3 forward)
    {
        Quaternion rot = Quaternion.LookRotation(forward);
        Instantiate(pickablePrefabs[type], pos, rot, itemHolder.transform);

        StartCoroutine(player.ResetItemCollider());
    }
    
    public void InstantiateTypeAtRandomSpherePos(Resource type, Vector3 pos)
    {
        Debug.Log("Instantiate item "+type+" at pos: "+pos);
        Instantiate(pickablePrefabs[(int)type], pos+Random.insideUnitSphere*0.18f, Quaternion.identity, itemHolder.transform);

        StartCoroutine(player.ResetItemCollider());
    }

}
