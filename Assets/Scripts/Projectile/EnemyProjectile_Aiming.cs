using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Aiming : Projectile//智能瞄准
{
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");//通过标签获取玩家位置
        SetTarget(target);
    }

    protected override void OnEnable()
    {
        StartCoroutine(MoveDirectionCorountine());
        base.OnEnable();
    }

    IEnumerator MoveDirectionCorountine()//子弹移动
    {
        yield return null;
        if (target.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }
}
