using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] int defaultAmount = 5;

    [SerializeField] GameObject missilePrefab = null;

    [SerializeField] AudioData launchSFX = null;

    [SerializeField] float cooldownTime = 1f;

    int amount;

    bool isReady = true;

    void Awake()
    {
        amount = defaultAmount;   
    }

    void Start()
    {
        MissileDisplay.UpdateAmountText(amount);
    }

    public void PickUp()
    {
        amount++;
        MissileDisplay.UpdateAmountText(amount);

        if(amount == 1)
        {
            MissileDisplay.UpdateCooldownImage(0f);
            isReady = true;
        }
    } 

    public void launch(Transform muzzleTransform)
    {
        if (amount == 0 || !isReady)return;


        isReady = false;
        PoolManager.Release(missilePrefab, muzzleTransform.position);

        AudioManager.Instance.PlayRandomSFX(launchSFX);

        amount--;

        MissileDisplay.UpdateAmountText(amount);

        if(amount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1);
        }
        else
        {
            StartCoroutine(CooldownCoroutine());
        }
    }

    IEnumerator CooldownCoroutine()
    {
        var cooldownValue = cooldownTime;

        while (cooldownValue > 0)
        {
            MissileDisplay.UpdateCooldownImage(cooldownValue/cooldownTime);
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime,0f);

            yield return null;
        }

        isReady = true;
    }
}
