using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] EnergyBar energyBar;//调用EnerBar类的方法

    [SerializeField] float overdriveInteral = 0.1f;//能量消耗时间间隔

    public const int MAX = 100;//最大能量值

    public const int PERCENT = 1;//最大能量百分比

    bool available = true;

    int energy;

    WaitForSeconds waitOverdriveInteral;

    protected override void Awake()
    {
        base.Awake();
        waitOverdriveInteral = new WaitForSeconds(overdriveInteral);
    }

    void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }

    void Start()
    {
        energyBar.Initialize(energy, MAX);//能量条初始化
        Obtain(MAX);
    }

    public void Obtain(int value)//获取能量
    {
        if (energy == MAX || !available || !gameObject.activeSelf)
            return;
        //energy += value;
        //energy = Mathf.Clamp(energy, 0, MAX);
        energy = Mathf.Clamp(energy + value, 0, MAX);//在限制范围内获获取能量
        energyBar.UpdateStats(energy,MAX);//获取后更新能量条
    }

    public void Use(int value)//使用能量
    {
        energy = Mathf.Clamp(energy - value, 0, MAX);
        energyBar.UpdateStats(energy,MAX);
        if(energy == 0 && !available)
        {
            PlayerOverdrive.off.Invoke();
        }
    }

    public bool IsEnough(int value) =>energy >= value;//检验能量是否够用

    void PlayerOverdriveOn()
    {
        StartCoroutine(nameof(KeepUsingCoroutine));
        available = false;
    }

    void PlayerOverdriveOff()
    {
        StopCoroutine(nameof(KeepUsingCoroutine));
        available = true;
    }

    IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf && energy > 0)
        {
            yield return waitOverdriveInteral;

            Use(PERCENT);
        }
    }

}
