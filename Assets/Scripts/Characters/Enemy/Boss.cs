using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    BossHealthBar healthBar;

    Canvas healthBarCanvas;

    protected override void Awake()
    {
        base.Awake();
        healthBar = FindObjectOfType<BossHealthBar>();  
        healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.Initialize(health, maxHealth);
        healthBarCanvas.enabled = true;
    }

    public override void Die()
    {
        healthBarCanvas.enabled = false;
        base.Die();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateStats(health,maxHealth);
    }

    protected override void SetHealth()
    {
        maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;
    }
}
