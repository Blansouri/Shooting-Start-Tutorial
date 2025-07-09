using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour//可继承的角色基类，用于生命值系统
{
    [SerializeField] GameObject deathVFX;//死亡特效

    [Header("Health")]
    [SerializeField] protected float maxHealth;//最大生命值

    [SerializeField] protected float health;//当前生命值

    [SerializeField] StatsBar onHeadHealthBar;

    [SerializeField] bool showOnHeadHealthBar = true;

    [SerializeField] AudioData[] deathSFX;

    protected virtual void OnEnable()//子类可访问、可重写
    {
        health = maxHealth;//对象启用时生命值为最大生命值
        if (showOnHeadHealthBar)
        {
            ShowOnHeadHealthBar();
        }
        else
        {
            HideOnHeadHealthBar();
        }
    }

    public void ShowOnHeadHealthBar()//启用头顶血条
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);
    }

    public void HideOnHeadHealthBar()//禁用头顶血条
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage)//受伤
    {
        if (health == 0) return;
        health -= damage;

        if (health <= 0f)
        {
            Die();
        }

        if (showOnHeadHealthBar&&gameObject.activeSelf)//显示血条且对象被启用
        {
            onHeadHealthBar.UpdateStats(health,maxHealth);
        }      
    }

    public virtual void Die()
    {
        health = 0f;
        PoolManager.Release(deathVFX, transform.position);//播放死亡特效
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        gameObject.SetActive(false);//将角色禁用
    }

    public virtual void RestoreHealth(float value)//生命值回复
    {
        if (health == maxHealth) return;

        health = Mathf.Clamp(health+value, 0f, maxHealth);//回复不超过最大生命值
        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }
    }

    protected IEnumerator HealthRegenerateCorouration(WaitForSeconds waitTime,float percent)//持续回复功能携程
    {
        while (health < maxHealth)
        {
            yield return waitTime;
            RestoreHealth(maxHealth*percent);
        }
    }

    protected IEnumerator DamageOverTimeCorouration(WaitForSeconds waitTime, float percent)//持续伤害功能携程
    {
        while (health > 0)
        {
            yield return waitTime;
            TakeDamage(health * percent);
        }
    }
}
