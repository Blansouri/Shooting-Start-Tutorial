using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;//时间刻度进度条

    float defaultFixedDeltaTime;

    float t;


    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        
    }

    public void Unpause() 
    {
        Time.timeScale = 1f;
        
    }
    public void BulletTime(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    public void BulletTime(float induration,float outduration)
    {
        StartCoroutine(SlowInAndOutCoroutine(induration,outduration));
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration,float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return StartCoroutine(SlowOutCoroutine(outDuration));
    }

    IEnumerator SlowInCoroutine(float duration)
    {
        float startTime = Time.unscaledDeltaTime;

        t = 0f;

        while (t < 1f)
        {
            if (GameManager.GameState!=GameState.Paused)
            {
            t += Time.unscaledDeltaTime / duration;
            Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            yield return null;
            }
            
        }

    }

    IEnumerator SlowOutCoroutine(float duration)
    {
        float startTime = Time.unscaledDeltaTime;

        t = 0f;

        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
            t += Time.unscaledDeltaTime / duration;
            Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            yield return null;
            }
            
        }

    }
}
