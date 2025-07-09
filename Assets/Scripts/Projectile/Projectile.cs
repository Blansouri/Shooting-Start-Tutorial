using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;//击中特效

    [SerializeField] protected float moveSpeed = 10f;

    [SerializeField] float damage;

    [SerializeField] protected Vector2 moveDirection;//移动方向

    [SerializeField] AudioData[] hitSFX; 

    protected GameObject target;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }

    IEnumerator MoveDirectly()//子弹前进携程
    {
        while (gameObject.activeSelf)//当子弹物体存在时
        {
            Move();
            yield return null;
        }

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)//子弹击中效果
    {
         //抓取到特定组件时返回true，相对GetComponent消耗性能更少
        if(collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);//造成伤害

            var contactPoint = collision.GetContact(0);//返回碰撞时第一个接触点
            //特效，碰撞点，根据接触点的法线方向的前方和上方返回一个旋转值
            PoolManager.Release(hitVFX, contactPoint.point,Quaternion.LookRotation(contactPoint.normal));

            AudioManager.Instance.PlayRandomSFX(hitSFX);

            gameObject.SetActive(false);//子弹禁用
        }
    }

    protected void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void Move()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
