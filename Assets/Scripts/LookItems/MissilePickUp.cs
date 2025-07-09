using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePickUp : LookItem
{
    protected override void PickUp()
    {
        player.PickMissile();
        base.PickUp();
    }
}
