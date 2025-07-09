using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField]int deathEnergyBonus = 3;//死亡能量奖励
    [SerializeField] int scorePoint = 100;
    [SerializeField] protected int healthFactor;

    LootSpawn lootSpawn;

    protected virtual void Awake()
    {
        lootSpawn = GetComponent<LootSpawn>();
    }

    protected override void OnEnable()
    {
        SetHealth();
        base.OnEnable();
    }

    public override void Die()//
    {
        ScoreManager.Instance.AddScore(scorePoint);
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);//敌人死亡获取能量
        EnemyManager.Instance.RemoveFromList(gameObject);
        lootSpawn.Spawn(transform.position);
        base.Die();
    }

    protected virtual void SetHealth()
    {
        maxHealth += (int)(EnemyManager.Instance.WaveNumber / healthFactor);
    }
}
