using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    int score;

    int currentScore;

    [SerializeField] Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);

    public void ResetScore()//重置分数
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateText(score);
    }

    public void AddScore(int scorePoint)//增加分数
    {
        currentScore += scorePoint;
        StartCoroutine(nameof(AddScoreCoroutine));
    }

    IEnumerator AddScoreCoroutine()//动态分数上升携程
    {
        ScoreDisplay.ScaleText(scoreTextScale);//对文本进行缩放
        while (score < currentScore)//逐帧增加分数文本
        {
            score += 1;
            ScoreDisplay.UpdateText(score);
            yield return null;
        }
        ScoreDisplay.ScaleText(Vector3.one);//缩放恢复
    }



}
