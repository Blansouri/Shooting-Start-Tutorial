using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawn : MonoBehaviour
{
    [SerializeField] LootSetting[] lootSettings;

    public void Spawn(Vector2 position)
    {
        foreach (var item in lootSettings)
        {
            item.Spawn(position+Random.insideUnitCircle);
        }
    }
}
