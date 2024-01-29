using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public static ItemCreator Instance;
    [SerializeField] PickableItem[] pickablePrefabs;
    [SerializeField] GameObject itemHolder;
    [SerializeField] Player player;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void InstantiateTypeAt(Resource type, Vector3 pos)
    {
        Debug.Log("Instantiate item "+type+" at pos: "+pos);
        Instantiate(pickablePrefabs[(int)type], pos+Random.insideUnitSphere*0.18f, Quaternion.identity, itemHolder.transform);

        StartCoroutine(player.ResetItemCollider());

    }

}
