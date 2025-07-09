using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerPickUp : LookItem
{
    [SerializeField] AudioData fullHealthPickUpSFX;

    [SerializeField] int fullHealthScoreBonus = 200;


    protected override void PickUp()
    {
        if (player.isFullPower)
        {
            pickUpSFX = fullHealthPickUpSFX;
            lootMessage.text = $"SCORE+{fullHealthScoreBonus}";
            ScoreManager.Instance.AddScore(fullHealthScoreBonus);
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = $"POWER UP!";
            player.PowerUp();
        }
        base.PickUp();
    }
}
