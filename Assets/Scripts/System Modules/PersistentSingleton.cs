using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component//持久单例
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        Instance = this as T;
        
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);//unity引擎加载新的场景时，不摧毁所挂载的游戏对象
    }

  
}
