using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    Text waveText;

    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;//获取世界相机
        waveText = GetComponentInChildren<Text>();//获取文本组件
    }

    void OnEnable()
    {
        waveText.text = "-WAVE" + EnemyManager.Instance.WaveNumber + "-";  
    }
}
