using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] Vector2 scrollVelocity;
    Material material;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    IEnumerator Start()
    {
        while (GameManager.GameState != GameState.GameOver) 
        {
         material.mainTextureOffset += scrollVelocity * Time.deltaTime;//位置加上偏移值，每秒更新。
            yield return null;        
        }                                                             
    }
}
