using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    [SerializeField] AudioData targetAcquireVoice = null;

    [SerializeField] AudioData explosionSFX = null;

    [SerializeField] GameObject explosionVFX = null;

    [SerializeField] LayerMask enemyLayerMask = default;

    [SerializeField] float lowSpeed = 8f;

    [SerializeField] float highSpeed = 25f;

    [SerializeField] float explosionDamage = 100f;

    [SerializeField] float variableSpeedDelay = 0.5f;

    [SerializeField] float explosionRadius = 3f;



    WaitForSeconds waitVariableSpeedDelay;

    protected override void Awake()
    {
        base.Awake();
        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedDelay));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        PoolManager.Release(explosionVFX,transform.position);

        AudioManager.Instance.PlayRandomSFX(explosionSFX);

        var colliders = Physics2D.OverlapCircleAll(transform.position,explosionRadius,enemyLayerMask);//检测圆形范围内的敌人对象

        foreach(var collider in colliders)
        {
            if(collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(explosionDamage);
            }
        }

    }

    IEnumerator VariableSpeedDelay()
    {
        moveSpeed = lowSpeed;

        yield return waitVariableSpeedDelay;

        moveSpeed = highSpeed;

        if(target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquireVoice);
        }
    }
}
