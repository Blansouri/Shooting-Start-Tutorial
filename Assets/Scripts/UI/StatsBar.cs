using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    
    [SerializeField] Image fillImageFront;

    [SerializeField] float fillSpeed = 0.1f;

    [SerializeField] float fillDelay = 0.5f;

    [SerializeField] bool delayFill = true;

    float currentFillAmount;//当前填充值

    float previousFillAmount;//初始填充值

    protected float targetFillAmount;//目标填充值

    float t;//线性插值的第三参数,声明为本地变量避免频繁触发垃圾回收机制

    WaitForSeconds waitForDelayFill;//等待填充时间

    Coroutine bufferedFillingCoroutine;

    Canvas canvas;

    void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;//将主摄像机设为画布的世界摄像机
        }
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    void OnDisable()
    {
        StopAllCoroutines();//条被禁用或被销毁时结束所有携程  
    }

    virtual public void Initialize(float currentValue,float maxValue)//状态条初始化,当前状态条和最大状态条
    {
        currentFillAmount = currentValue/maxValue;

        targetFillAmount = currentFillAmount;

        fillImageBack.fillAmount = currentFillAmount;

        fillImageFront.fillAmount = currentFillAmount;
    }

    public void UpdateStats(float currentValue, float maxValue)//更新状态条
    {
        if (bufferedFillingCoroutine != null)//避免重复多开携程，每次启用前要关闭已启用的携程
        {
            StopCoroutine(bufferedFillingCoroutine);
        }
        

        targetFillAmount = currentValue/maxValue;

            if (currentFillAmount > targetFillAmount)//当前状态值减少
            {
                fillImageFront.fillAmount = targetFillAmount;
                bufferedFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageBack));
            }

            else //当前状态值增加
            {
                fillImageBack.fillAmount = targetFillAmount;
                bufferedFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageFront));
            }
             
    }

    protected virtual IEnumerator BufferFillingCoroutine(Image image)//条缓慢填充
    {
        if (delayFill)//挂起等待填充时间
        {
            yield return waitForDelayFill;
        }

        t = 0;

        previousFillAmount = currentFillAmount;

        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed ;

            currentFillAmount = Mathf.Lerp(previousFillAmount,targetFillAmount,t);

            image.fillAmount =currentFillAmount;

            yield return null;

        }
        
    }
}
