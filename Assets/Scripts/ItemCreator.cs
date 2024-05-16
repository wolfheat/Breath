using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public static ItemCreator Instance;
    [SerializeField] ResourceItem[] resourcePrefabs;
    [SerializeField] DestructableItem[] destructablePrefabs;
    [SerializeField] EnemyController[] enemyPrefabs;
    [SerializeField] GameObject itemHolder;
    [SerializeField] Player player;
    [SerializeField] GameObject[] destructablesHolders;
    [SerializeField] GameObject[] resourceHolders;
    [SerializeField] GameObject[] enemyHolders;
    [SerializeField] GameObject pickablesHolder;
    [SerializeField] PickableItem[] genericPrefabs;
    [SerializeField] InventoryItemsDatabase inventoryItemsDatabase;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    
    
    public void DefineGameDataForSave()
    {
        // Save all items to file

        // Save Destructables, Resources, Pickables and Enemies
        SavingUtility.playerGameData.Destructables = ReadGame<DestructableItem, ItemData, SaveItem>(destructablesHolders); 
        SavingUtility.playerGameData.Resources = ReadGame<ResourceItem, ItemData, SaveItem>(resourceHolders);
        SavingUtility.playerGameData.Enemies = ReadGame<EnemyController, EnemyData, SaveEnemy>(enemyHolders);

        SavingUtility.playerGameData.Pickables = ReadGame(pickablesHolder);
    }

    private SaveDroppedItem[] ReadGame(GameObject pickablesHolder)
    {
        // Get All children of type BaseObjectWithType T
        PickableItem[] items = pickablesHolder.GetComponentsInChildren<PickableItem>();
        SaveDroppedItem[] saveDroppedItems = new SaveDroppedItem[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            PickableItem item = items[i];
            int subType = 0;
            if (item.Data is ConsumableData cons)
                subType = cons.SubType;
            else if (item.Data is EquipableData equip)
                subType = equip.SubType;
            saveDroppedItems[i] = new SaveDroppedItem() { mainType = item.Data.Type, subType = subType, position = SavingUtility.Vector3AsV3(item.transform.position), rotation = SavingUtility.QuaternionAsV4(item.transform.rotation) };
        }
        Debug.Log("    Saved " + items.Length + " " + typeof(PickableItem) + (items.Length == 1 ? "" : "s") + " from " + pickablesHolder.name);
        return saveDroppedItems;
    }
    
    private void LoadSaveDroppedItems(SaveDroppedItem[] saveDroppedItems)
    {
        // Generate correct itemdata and make a generic item

        foreach(var saveDrop in saveDroppedItems)
        {
            ItemData data = inventoryItemsDatabase.GetData((ItemType)saveDrop.mainType, saveDrop.subType);
            if (data == null) continue;

            InstantiateGenericItemAt(data, SavingUtility.V3AsVector3(saveDrop.position), SavingUtility.V4AsQuaternion(saveDrop.rotation));            
        }
        Debug.Log("    Loaded " + saveDroppedItems.Length + " " + typeof(SaveDroppedItem) + (saveDroppedItems.Length == 1 ? "" : "s") + " into " + pickablesHolder.name);
    }

    // T = DestructableItem, PickableItem, EnemyController etc
    public S[][] ReadGame<T, D, S>(GameObject[] holders) where T : BaseObjectWithType where D : BaseData where S : SaveItem, new()
    {
        // Save Objects into SaveItem S
        S[][] save = new S[holders.Length][];
        for (int i = 0; i < holders.Length; i++)
        {
            // Get All children of type BaseObjectWithType T
            T[] items = holders[i].GetComponentsInChildren<T>();

            List<S> itemsList = new List<S>();
            foreach (var item in items)
                itemsList.Add(new S() { id = item.Type, position = SavingUtility.Vector3AsV3(item.transform.position), rotation = SavingUtility.QuaternionAsV4(item.transform.rotation) });

            save[i] = itemsList.ToArray();
            Debug.Log("    Saved " + save[i].Length + " " + typeof(T) + (save[i].Length == 1 ? "" : "s") + " from " + holders[i].name);
        }
        return save;

    }
    public void LoadFromFile()
    {
        Debug.Log("  * LOADING ALL OBJECTS FROM FILE *");
        // Clear all old data
        ClearAllSaveObjects();

        InstantiateObjectsInGame(destructablesHolders, SavingUtility.playerGameData.Destructables, destructablePrefabs);
        InstantiateObjectsInGame(resourceHolders, SavingUtility.playerGameData.Resources, resourcePrefabs);
        InstantiateObjectsInGame(enemyHolders, SavingUtility.playerGameData.Enemies, enemyPrefabs);

        LoadSaveDroppedItems(SavingUtility.playerGameData.Pickables);
    }

    private void ClearAllSaveObjects()
    {
        foreach (var holder in destructablesHolders)
            foreach (Transform item in holder.transform)
                Destroy(item.gameObject);
        foreach (var holder in resourceHolders)
            foreach (Transform item in holder.transform)
                Destroy(item.gameObject);
        foreach (var holder in enemyHolders)
            foreach (Transform enemy in holder.transform)
                Destroy(enemy.gameObject);
        Debug.Log("    All previous items destroyed");
    }

    // T = DestructableItem, PickableItem, EnemyController etc
    public void InstantiateObjectsInGame<T, S>(GameObject[] holders, S[][] Objects, T[] prefabs) where T : BaseObjectWithType where S : SaveItem
    {
        for (int i = 0; i < Objects.Length; i++)
        {
            foreach (var obj in Objects[i])
                Instantiate(prefabs[obj.id], SavingUtility.V3AsVector3(obj.position), SavingUtility.V4AsQuaternion(obj.rotation), holders[i].transform);
            Debug.Log("    Loaded " + Objects[i].Length + " "+ typeof(T)+ (Objects[i].Length == 1 ? "" : "s") + " into " + holders[i].name);
        }
    }

    public void InstantiateGenericItemAt(ItemData itemData, Vector3 pos, Quaternion rot)
    {
        PickableItem item = Instantiate(genericPrefabs[0], pos, rot, pickablesHolder.transform);
        item.Data = itemData;
    }
    public void InstantiateResourceAt(Resource type, Vector3 pos, Vector3 forward)
    {
        InstantiateResourceAt((int) type, pos, forward);
    }
    public void InstantiateResourceAt(int type, Vector3 pos, Vector3 forward)
    {
        Quaternion rot = Quaternion.LookRotation(forward);
        Instantiate(resourcePrefabs[type], pos, rot, itemHolder.transform);
    }
    
    public IEnumerator InstantiateTypeAtRandomSpherePos(Resource[] types, Vector3 randomPointCenter)
    {
        yield return null;
        foreach (var type in types)
            InstantiateTypeAtRandomSpherePos(type, randomPointCenter);
    }
    public void InstantiateTypeAtRandomSpherePos(Resource type, Vector3 randomPointCenter)
    {
        // Try instantiating at random positions 20 times to not overlap colliders 
        int tries = 0;
        float creationRadius = 0.3f;
        Vector3 pos = new Vector3(); 
        while (tries < 20)
        {
            pos = randomPointCenter + Random.insideUnitSphere * creationRadius;
            Collider[] colliders = Physics.OverlapSphere(pos, ResourceItem.ResourceSize*0.5f);
            if (colliders.Length==0 || ColliderIsNotEnvironment(colliders))
                break;
            
            tries++;
            if (tries == 10)
                creationRadius *= 2;
        }
        Debug.Log("Created resource "+type+" after "+tries+ " attempts");
        Instantiate(resourcePrefabs[(int)type], pos, Quaternion.identity, itemHolder.transform);
    }

    private bool ColliderIsNotEnvironment(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
            if (collider.gameObject.layer == 0)
                return false;
        return true;
    }

}
