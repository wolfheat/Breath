using UnityEngine;
public enum EnemyType { SpiderBoss, Blob }

[CreateAssetMenu(menuName = "Units/EnemyData", fileName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public EnemyType enemyType = EnemyType.SpiderBoss;
}
