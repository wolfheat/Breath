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

    
    
    public void DefineGameDataForSave()
    {
        // Save all items to file

        // Save Destructables, Pickables and Enemies
        SavingUtility.playerGameData.Destructables = ReadGame<DestructableItem, ItemData, SaveItem>(destructablesHolders); 
        SavingUtility.playerGameData.Resources = ReadGame<ResourceItem, ItemData, SaveItem>(pickablesHolders);
        SavingUtility.playerGameData.Enemies = ReadGame<EnemyController, EnemyData, SaveEnemy>(enemyHolders);
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
        }
        return save;

    }
    public void LoadFromFile()
    {
        // Clear all old data
        ClearAllSaveObjects();

        InstantiateObjectsInGame(destructablesHolders, SavingUtility.playerGameData.Destructables, destructablePrefabs);
        InstantiateObjectsInGame(pickablesHolders, SavingUtility.playerGameData.Resources, resourcePrefabs);
        InstantiateObjectsInGame(enemyHolders, SavingUtility.playerGameData.Enemies, enemyPrefabs);
    }

    private void ClearAllSaveObjects()
    {
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
    }

    // T = DestructableItem, PickableItem, EnemyController etc
    public void InstantiateObjectsInGame<T, S>(GameObject[] holders, S[][] Objects, T[] prefabs) where T : BaseObjectWithType where S : SaveItem
    {
        for (int i = 0; i < Objects.Length; i++)
            foreach (var obj in Objects[i])
            {
                Debug.Log("Loading object of type "+obj.id+" "+typeof(T));
                Instantiate(prefabs[obj.id], SavingUtility.V3AsVector3(obj.position), SavingUtility.V4AsQuaternion(obj.rotation), holders[i].transform);
            }
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
        Debug.Log("Instantiate COroutine runs "+types.Length);
        yield return null;
        Debug.Log("Instantiate COroutine runs "+types.Length);
        foreach (var type in types)
            InstantiateTypeAtRandomSpherePos(type, randomPointCenter);
    }
    public void InstantiateTypeAtRandomSpherePos(Resource type, Vector3 randomPointCenter)
    {

        // Try instantiating at random positions 10 times to not overlap colliders
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
