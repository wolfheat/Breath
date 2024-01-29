using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletCreator : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    public static BulletCreator Instance { get; private set; }

    private void Start()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    public void GenerateBullet(Transform fromObject)
    {
        Bullet bullet = Instantiate(bulletPrefab, fromObject.position,fromObject.rotation,transform);
        


    }
}
