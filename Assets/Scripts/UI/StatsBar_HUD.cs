using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar_HUD : StatsBar
{
    [SerializeField] public Text percentText;

    protected virtual void SetPercent()
    {
        percentText.text = Mathf.RoundToInt(targetFillAmount * 100f) + "%";//显示护盾百分比
    }

    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPercent();
    }

    protected override IEnumerator BufferFillingCoroutine(Image image)
    {
        SetPercent();
        return base.BufferFillingCoroutine(image);
    }
}
