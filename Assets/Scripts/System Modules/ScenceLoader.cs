using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenceLoader : PersistentSingleton<ScenceLoader>
{
    [SerializeField] Image transitionImage;//转场图片

    [SerializeField] float fadeTime = 3.5f;//转场时间

    Color color;//转场画面颜色
 
    const string GAMEPLAY = "Gameplay";

    const string MAIN_MENU = "MainMenu";

    IEnumerator LoadingCoroutine(string sceneName)//转换时淡入淡出效果
    {
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);

        loadingOperation.allowSceneActivation = false;

        transitionImage.gameObject.SetActive(true);//激活转场图片

        while (color.a < 1f)//画面淡出
        {
            color.a = Mathf.Clamp01( color.a += Time.unscaledDeltaTime / fadeTime);//在0-1范围内修改图片的a值(曝光度)
            transitionImage.color = color;

            yield return null;
        }

        loadingOperation.allowSceneActivation = true;

        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);

        

        while (color.a > 0)//画面淡入
        {
            color.a = Mathf.Clamp01(color.a -= Time.unscaledDeltaTime / fadeTime);//在0-1范围内修改图片的a值(曝光度)
            transitionImage.color = color;

            yield return null;
        }

        transitionImage.gameObject.SetActive(false);
    }

    public void LoadGamePlayScene()//导入对应场景
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }

    public void LoadMainScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAIN_MENU));
    }
}
