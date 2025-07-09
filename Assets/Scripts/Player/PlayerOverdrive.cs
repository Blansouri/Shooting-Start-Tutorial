using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction on = delegate { };//爆发开启委托

    public static UnityAction off = delegate { };//爆发关闭委托

    [SerializeField] GameObject triggerVFX;

    [SerializeField] GameObject engineVFXNormal;

    [SerializeField] GameObject engineVFXOverdrive;

    [SerializeField] AudioData onSFX;

    [SerializeField] AudioData offSFX;

    void Awake()
    {
        on += On;
        off += Off;
    }

    void OnDestroy()
    {
        on -= On;
        off -= Off;
    }

    void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(true);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }

    void Off()
    {
        triggerVFX.SetActive(false);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(false);
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }
}
