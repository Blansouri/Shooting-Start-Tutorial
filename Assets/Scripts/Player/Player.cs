using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] StatsBar_HUD statsBar_HUD;//HUD血条

    [SerializeField] bool regenerateHealth = true;//生命值回复开关

    [SerializeField] float healthRegenerateTime;//生命值回复时间

    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;//生命值回复百分比

    [Header("---Input---")]

    [SerializeField] PlayerInput input;

    [Header("---Move---")]

    [SerializeField] float moveSpeed = 10f;//初始速度，可在编辑器修改

    [SerializeField] float accelerationTime = 3f;//加速时间

    [SerializeField] float decelerationTime = 3f;//减速时间

    [SerializeField] float paddingX = 0.8f;

    [SerializeField] float paddingY = 0.23f;

    [SerializeField] float moveRotationAngle = 30f;

    [Header("---Fire---")]

    [SerializeField] GameObject projectile1;//弹丸

    [SerializeField] GameObject projectile2;

    [SerializeField] GameObject projectile3;

    [SerializeField] GameObject projectileOverdrive;

    [SerializeField, Range(0, 2)] int weaponPower = 0;//武器威力

    [SerializeField] Transform muzzleMiddle;

    [SerializeField] Transform muzzleTop;

    [SerializeField] Transform muzzleBottom;//枪口

    [SerializeField] float FireInterval = 0.2f;//发射时间间隔

    [SerializeField] AudioData projectileLunchSFX;

    [Header("---Dodge---")]

    [SerializeField, Range(0, 100)] int dodgeEnergyCost = 25;//闪避消耗能量

    [SerializeField] float maxRoll = 720f;//最大滚转角

    [SerializeField] float rollSpeed = 360f;//滚转角速度

    [SerializeField] AudioData dodgeLunchSFX;

    [Header("--Overdrive")]

    [SerializeField] int overdriveDodgeFactor = 2;//开启爆发后对应属性改变的倍数

    [SerializeField] float overdriveSpeedFactor = 1.2f;

    [SerializeField] float overdriveFireFactor = 1.2f;

    float currentRoll;//当前滚转角

    bool isDodging = false;

    bool isOverdrive = false;

    readonly float slowMotionDuration = 1f; 

    float t;

    Vector2 previousVelocity;

    Quaternion previousRotation;

    WaitForSeconds waitForFireInterval;//开火间隔

    WaitForSeconds waitHealthRegenerateTime;//生命恢复间隔

    WaitForSeconds waitForOverdriveFireInerval;

    WaitForSeconds waitDecelerationTime;

    WaitForSeconds waitInvincibleTime;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    Coroutine moveCoroutine;

    Coroutine healthRegenerateCoroutine;

    new Rigidbody2D rigidbody;//调用刚体组件

    new Collider2D collider;

    MissileSystem missile;

    public bool isFullHealth => health == maxHealth;

    public bool isFullPower => weaponPower == 2;

    [SerializeField] float InvincibleTime = 1f;//无敌时间

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;
        input.onLaunchMissile += LaunchMissile;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }

    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;
        input.onLaunchMissile -= LaunchMissile;

        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }

    void Start()
    {
        rigidbody.gravityScale = 0f;//刚体重力为零

        waitForFireInterval = new WaitForSeconds(FireInterval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitForOverdriveFireInerval = new WaitForSeconds(FireInterval/overdriveFireFactor);
        waitDecelerationTime = new WaitForSeconds(decelerationTime);
        waitInvincibleTime = new WaitForSeconds(InvincibleTime);

        statsBar_HUD.Initialize(health, maxHealth);//hud血条初始化
        
        input.EnableGameplayInput();//Player脚本运行时调用GamePlay动作表
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        statsBar_HUD.UpdateStats(health, maxHealth);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        statsBar_HUD.UpdateStats(health, maxHealth);

        if (regenerateHealth)
        {

            if (healthRegenerateCoroutine != null)
            {
                StopCoroutine(healthRegenerateCoroutine);
            }
            if (gameObject.activeSelf)
            {
                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCorouration(waitHealthRegenerateTime, healthRegeneratePercent));
                StartCoroutine(InvincibleCoroutine());
            }
        }
    }

    public override void Die()
    {
        GameManager.onGameOver?.Invoke();//死亡后进入调用游戏结束委托
        GameManager.GameState = GameState.GameOver;
        statsBar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
    }

    IEnumerator InvincibleCoroutine()
    {
        collider.isTrigger = true;

        yield return waitInvincibleTime;

        collider.isTrigger = false;
    }


    //// Update is called once per frame
    //void Update()
    //{
    //    //transform.position=Viewport.Instance.PlayerMoveablePosition(transform.position);
    //    //调用单例Viewport限制视口范围，但由于update每一帧都调用，消耗性能太高，换携程来调用
    //}

    #region MOVE
    void Move(Vector2 moveInput)//移动事件处理
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);//返回特定轴上的角度
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed, moveRotation));//传输速度
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MovePositionLimitCoroutine));//在操作角色时调用位置限制携程
    }

    void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);//停止携程
        }

        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        StartCoroutine(nameof(DecelerationCoroutine));//角色停下时停止调用携程
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)//角色加减速和翻转角度的携程
    {
        t = 0f;
        previousVelocity = rigidbody.velocity;
        previousRotation = transform.rotation;
        while (t < time)
        {
            t += Time.fixedDeltaTime;
            rigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t / time);//角色加减速
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t / time);//角度翻转

            yield return  waitForFixedUpdate; 
        }
    }

    IEnumerator MovePositionLimitCoroutine()//携程限定角色范围
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);
            yield return null;
        }
    }

    IEnumerator DecelerationCoroutine()
    {
        yield return waitDecelerationTime;
        StopCoroutine(nameof(MovePositionLimitCoroutine));
    }

    #endregion

    #region Fire
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }
    void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(isOverdrive ? projectileOverdrive: projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile1, muzzleTop.position);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile1, muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile2, muzzleTop.position);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile3, muzzleBottom.position);
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayRandomSFX(projectileLunchSFX);//播放开火音频

            yield return isOverdrive ? waitForOverdriveFireInerval : waitForFireInterval;
            //if (isOverdrive)
            //    yield return waitForOverdriveFireInerval;
            //else
            //    yield return waitForFireInterval;
        }
    }
    #endregion

    #region Dodge
    void Dodge()
    {
        if (isDodging|| !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;//正在闪避或者能量不足
        StartCoroutine(DodgeCoroutine());
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true;//处于闪避状态

        AudioManager.Instance.PlayRandomSFX(dodgeLunchSFX);

        PlayerEnergy.Instance.Use(dodgeEnergyCost);//闪避消耗能量

        collider.isTrigger = true;//玩家无敌启用

        float moveSpeedSave = moveSpeed;

        moveSpeed = moveSpeed * 2f;//玩家加速

        currentRoll = 0;//每次闪避前将滚转角设为0

        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation  = Quaternion.AngleAxis(currentRoll, Vector3.right);
            yield return null;
        }

        collider.isTrigger = false;//玩家无敌关闭

        moveSpeed = moveSpeedSave;//加速关闭

        isDodging = false;//不处于闪避状态
    }
    #endregion

    #region Overdrive
    void Overdrive()
    {
        if(!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        PlayerOverdrive.on.Invoke();
    }

    void OverdriveOn()
    {
        isOverdrive = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);
    }

    void OverdriveOff()
    {
        isOverdrive = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }
    #endregion

    void LaunchMissile()
    {
        missile.launch(muzzleMiddle);
    }

    public void PickMissile()
    {
        missile.PickUp();
    }

    internal void PowerUp()
    {
        weaponPower += 1;
        weaponPower = Math.Clamp(weaponPower, 0, 2);
    }
}
