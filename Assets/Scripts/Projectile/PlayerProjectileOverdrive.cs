using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] ProjectileGuidanceSystem guidanceSystem;

    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);

        transform.rotation = Quaternion.identity;//子弹旋转值恢复到默认角度

        if (target == null)
            base.OnEnable();
        else
        {
            StartCoroutine(guidanceSystem.HomingCoroutine(target));
        }
    }
}
