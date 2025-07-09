using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component//泛型约束Component是所有能挂载到游戏对象上的基类,泛型单例能够让别的脚本直接继承
{
    public static T Instance { get; private set; }

    protected virtual void Awake()//可重写
    {
        Instance = this as T;
    }
}
