using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] float continuousFireDuration = 1.5f;

    [Header("---Player Detection---")]

    [SerializeField] Transform playerDetectionTransform;

    [SerializeField] Vector3 playDetectionSize;

    [SerializeField] LayerMask playerLayer;

    [Header("---Beam---")]

    [SerializeField] float beamCooldownTime = 20f;

    [SerializeField] bool isBeamReady = false;

    [SerializeField] AudioData beamChargingSFX;

    [SerializeField] AudioData beamlaunchSFX;

    int launchBeamID = Animator.StringToHash("launchBeam");

    WaitForSeconds waitForContinuousFireInterval;

    WaitForSeconds waitForFireInterval;

    WaitForSeconds waitBeanCooldownTime;

    List<GameObject> magazine;

    AudioData launchSFX;

    Animator animator;

    Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);
        waitForFireInterval =new WaitForSeconds(maxFireInterval);
        waitBeanCooldownTime = new WaitForSeconds(beamCooldownTime);
        magazine = new List<GameObject>(projectiles.Length);

        playerTransform =GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void OnEnable()
    {
        isBeamReady = false;
        StartCoroutine(nameof(BeamCoolDownCoroutine));
        base.OnEnable();

    }

    void LoadProjectiles()
    {
        magazine.Clear();
        if (Physics2D.OverlapBox(playerDetectionTransform.position,playDetectionSize,0,playerLayer))
        {
            magazine.Add(projectiles[0]);
            launchSFX = projectileLunchSFX[0];
        }
        else
        {
            if (Random.value < 0.5f)
            {
                magazine.Add(projectiles[1]);
                launchSFX = projectileLunchSFX[1];
            }
            else
            {
                for( int i = 2; i < projectiles.Length; i++)
                {
                    magazine.Add(projectiles[i]);
                }
                launchSFX = projectileLunchSFX[2];
            }
        }
    }

    void ActivateBeamWeapon()
    {
        isBeamReady = false;
        animator.SetTrigger(launchBeamID);
        AudioManager.Instance.PlayRandomSFX(beamChargingSFX);
    }

    void LaunchBean()
    {
        AudioManager.Instance.PlayRandomSFX(beamlaunchSFX);
    }

    void StopBeam()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));
        StartCoroutine(nameof(BeamCoolDownCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    protected override IEnumerator RandomlyFireCoroutine()
    {
        
        while (isActiveAndEnabled)
        {
            if (GameManager.GameState == GameState.GameOver) yield break;
            if (isBeamReady)
            {
                ActivateBeamWeapon();
                StartCoroutine(nameof(ChasingPlayerCoroutine));
                yield break;
            }

            yield return waitForFireInterval;
            yield return StartCoroutine(ContinuousFireCoroutine());
        }
    }

    IEnumerator ContinuousFireCoroutine()
    {
        LoadProjectiles();

        float continuousFireTimer = 0f;

        while(continuousFireTimer < continuousFireDuration)
        {
            foreach (var projectile in magazine)
            {
                PoolManager.Release(projectile,muzzle.position);
            }

            continuousFireTimer += minFireInterval;
            AudioManager.Instance.PlayRandomSFX(launchSFX);

            yield return waitForContinuousFireInterval;
        }
    }

    IEnumerator BeamCoolDownCoroutine()
    {
        yield return waitBeanCooldownTime;

        isBeamReady = true ;
    }

    IEnumerator ChasingPlayerCoroutine()
    {
        while (isActiveAndEnabled)
        {
            targetPosition.x = Viewport.Instance.MaxX - paddingX;
            targetPosition.y = playerTransform.position.y;

            yield return null;
        }
    }
}
