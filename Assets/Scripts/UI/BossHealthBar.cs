using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : StatsBar_HUD
{
    protected override void SetPercent()
    {
        percentText.text = (targetFillAmount * 100f).ToString("f2") + "%";

    }
}
