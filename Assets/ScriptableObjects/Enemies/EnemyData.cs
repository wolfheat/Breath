using UnityEngine;
public enum EnemyType { SpiderBoss, Blob }

[CreateAssetMenu(menuName = "Units/EnemyData", fileName = "Enemy")]
public class EnemyData : BaseData
{
    public EnemyType enemyType = EnemyType.SpiderBoss;
    public override int Type => (int)enemyType;
}

public abstract class BaseData : ScriptableObject
{
    public abstract int Type { get;}
}
