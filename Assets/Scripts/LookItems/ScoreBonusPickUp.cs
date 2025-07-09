using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBonusPickUp : LookItem
{
    [SerializeField] int scoreBonus;

    protected override void PickUp()
    {
        ScoreManager.Instance.AddScore(scoreBonus);
        base.PickUp();
    }
}
