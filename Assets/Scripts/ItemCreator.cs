using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public static ItemCreator Instance;
    [SerializeField] PickableItem[] pickablePrefabs;
    [SerializeField] DestructableItem[] destructablePrefabs;
    [SerializeField] EnemyController[] enemyPrefabs;
    [SerializeField] GameObject itemHolder;
    [SerializeField] Player player;
    [SerializeField] GameObject[] destructablesHolders;
    [SerializeField] GameObject[] pickablesHolders;
    [SerializeField] GameObject[] enemyHolders;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    // T = DestructableItem, PickableItem, EnemyController etc
    public S[][] ReadGame<T,D,S>(GameObject[] holders) where T : MonoBehaviour where D : BaseData where S : SaveItem,new()
    {

        // Save Destructables
        S[][] save = new S[holders.Length][];
        for (int i = 0; i < holders.Length; i++)
        {
            GameObject holder = holders[i];
            // Get All Destructable children
            T[] items = holder.GetComponentsInChildren<T>();

            List<S> itemsList = new List<S>();
            foreach (var item in items)
            {
                if (item.gameObject.activeSelf)
                {
                    int id = 0;
                    if (item is DestructableItem)
                        id = (item as DestructableItem).Data.Type;
                    else if (item is PickableItem)
                        id = (item as PickableItem).Data.Type;
                    else if (item is EnemyController)
                        id = (item as EnemyController).Data.Type;

                    itemsList.Add(new S() { id = id, position = SavingUtility.Vector3AsV3(item.transform.position), forward = SavingUtility.Vector3AsV3(item.transform.forward), up = SavingUtility.Vector3AsV3(item.transform.up) });
                }
            }
            save[i] = itemsList.ToArray();
        }
        return save;

    }
    public void SetGameData()
    {
        // Save all items to file

        // Save Destructables
        SaveItem[][] destructableSave = ReadGame<DestructableItem,ItemData,SaveItem>(destructablesHolders);
        SavingUtility.playerGameData.Destructables = destructableSave;

        SaveItem[][] pickableSave = ReadGame<PickableItem, ItemData, SaveItem>(pickablesHolders);
        SavingUtility.playerGameData.Pickables = pickableSave;

        SaveEnemy[][] enemySave = ReadGame<EnemyController, EnemyData, SaveEnemy>(enemyHolders); 
        SavingUtility.playerGameData.Enemies = enemySave;

        /*
        SaveItem[][] destructableSave = new SaveItem[destructablesHolders.Length][];
        for (int i = 0; i < destructablesHolders.Length; i++)
        {
            GameObject holder = destructablesHolders[i];
            // Get All Destructable children
            DestructableItem[] items = holder.GetComponentsInChildren<DestructableItem>();
            Debug.Log("  Found "+items.Length+" items in holder "+holder.name);
            List<SaveItem> saveItems = new List<SaveItem>();
            foreach (var item in items)
            {
                if (item.gameObject.activeSelf)
                    saveItems.Add(new SaveItem() { id = GetItemID(item.Data as DestructableData), position = SavingUtility.Vector3AsV3(item.transform.position), forward = SavingUtility.Vector3AsV3(item.transform.forward) });
            }
            destructableSave[i] = saveItems.ToArray();
        }
        SavingUtility.playerGameData.Destructables = destructableSave;

        // Save Pickables
        SaveItem[][] pickableSave = new SaveItem[pickablesHolders.Length][];
        for (int i = 0; i < pickablesHolders.Length; i++)
        {
            GameObject holder = pickablesHolders[i];
            // Get All Destructable children
            PickableItem[] items = holder.GetComponentsInChildren<PickableItem>();
            Debug.Log("  Found "+items.Length+" items in holder "+holder.name);
            List<SaveItem> saveItems = new List<SaveItem>();
            foreach (var item in items)
            {
                if (item.gameObject.activeSelf)
                    saveItems.Add(new SaveItem() { id = GetItemID(item.Data as ResourceData), position = SavingUtility.Vector3AsV3(item.transform.position), forward = SavingUtility.Vector3AsV3(item.transform.forward) });
            }
            pickableSave[i] = saveItems.ToArray();
        }
        SavingUtility.playerGameData.Pickables = pickableSave;

        // Save Enemies
        SaveEnemy[][] enemySave = new SaveEnemy[enemyHolders.Length][];
        for (int i = 0; i < enemyHolders.Length; i++)
        {
            GameObject holder = enemyHolders[i];
            // Get All Destructable children
            EnemyController[] enemies = holder.GetComponentsInChildren<EnemyController>();
            Debug.Log("  Found "+ enemies.Length+" items in holder "+holder.name);
            List<SaveEnemy> saveEnemies = new List<SaveEnemy>();
            foreach (var enemy in enemies)
            {
                if (enemy.gameObject.activeSelf)
                {
                    (Vector3 pos, Vector3 forward, Vector3 up) = enemy.GetLocation();
                    saveEnemies.Add(new SaveEnemy() { id = GetItemID(enemy.Data as EnemyData), position = SavingUtility.Vector3AsV3(pos), forward = SavingUtility.Vector3AsV3(forward), up = SavingUtility.Vector3AsV3(up) });
                    Debug.Log("Saving Rotation forward: " + forward + " Up: " + up);
                }
            }
            enemySave[i] = saveEnemies.ToArray();
        }
        SavingUtility.playerGameData.Enemies = enemySave;
        */
    }

    private int GetItemID(EnemyData item)
    {
        return (int)item.enemyType;
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
        foreach (var holder in enemyHolders)
            foreach (Transform enemy in holder.transform)
                Destroy(enemy.gameObject);

        Debug.Log("  All items destroyed");
        SaveItem[][] destructableLoad = SavingUtility.playerGameData.Destructables;
        for (int i = 0; i < destructableLoad.Length; i++)
        {
            foreach (var item in destructableLoad[i])
            {
                Quaternion rotation = Quaternion.LookRotation(SavingUtility.V3AsVector3(item.forward), SavingUtility.V3AsVector3(item.up));
                Instantiate(destructablePrefabs[item.id], SavingUtility.V3AsVector3(item.position),rotation, destructablesHolders[i].transform);
            }
            Debug.Log("  Created "+ destructableLoad[i].Length + " Deastructables for Area " + (i + 1));
        }

        SaveItem[][] pickableLoad = SavingUtility.playerGameData.Pickables;
        for (int i = 0; i < pickableLoad.Length; i++)
        {
            foreach (var item in pickableLoad[i])
            {
                Quaternion rotation = Quaternion.LookRotation(SavingUtility.V3AsVector3(item.forward), SavingUtility.V3AsVector3(item.up));
                Instantiate(pickablePrefabs[item.id], SavingUtility.V3AsVector3(item.position),rotation, pickablesHolders[i].transform);
            }
            Debug.Log("  Created " + pickableLoad[i].Length + " Pickables for Area " + (i + 1));

        }

        // Load Enemies
        SaveEnemy[][] enemiesLoad = SavingUtility.playerGameData.Enemies;
        for (int i = 0; i < enemiesLoad.Length; i++)
        {
            foreach (var enemy in enemiesLoad[i])
            {
                EnemyController newEnemy = Instantiate(enemyPrefabs[enemy.id], enemyHolders[i].transform);
                newEnemy.SetLocation(SavingUtility.V3AsVector3(enemy.position), SavingUtility.V3AsVector3(enemy.forward), SavingUtility.V3AsVector3(enemy.up));
                Debug.Log("Loading Rotation Forward: " + SavingUtility.V3AsVector3(enemy.forward)+" Up: "+ SavingUtility.V3AsVector3(enemy.up));

            }
            Debug.Log("  Created " + enemiesLoad[i].Length + " Enemies for Area " + (i + 1));

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
