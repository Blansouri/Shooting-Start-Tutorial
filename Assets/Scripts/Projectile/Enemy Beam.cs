using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    [SerializeField] float damage =50f;

    [SerializeField] GameObject hitVFX;

    void OnCollisionStay2D(Collision2D collision)//子弹击中效果
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))//抓取到特定组件时返回true，相对GetComponent消耗性能更少
        {
            player.TakeDamage(damage);//造成伤害

            var contactPoint = collision.GetContact(0);//返回碰撞时第一个接触点

            PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));//特效，碰撞点，根据接触点的法线方向的前方和上方返回一个旋转值
        }
    }
}
