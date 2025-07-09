using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport :Singleton<Viewport>//继承Singleto类
{
    float minX;
    float maxX;
    float minY;
    float maxY;
    float middleX;

    public float MaxX => maxX;

    void Start()//将摄像机范围坐标转化为(0,0)(1,1)的世界坐标
    {
        Camera mainCamera = Camera.main;  

        Vector2 bottlemLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));//将左下角坐标转换为世界坐标

        minX = bottlemLeft.x;
        minY = bottlemLeft.y;

        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));//将右上角坐标转化为世界坐标

        maxX = topRight.x;
        maxY = topRight.y;

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;
    }
     
    public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)//得到限定玩家移动的范围值
    {
        Vector3 position =Vector3.zero;

        position.x = Mathf.Clamp(playerPosition.x, minX+paddingX, maxX-paddingX);

        position.y = Mathf.Clamp(playerPosition.y, minY+paddingY, maxY-paddingY);

        return position;
    }

    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)//敌人随机生成的初始位置
    {
        Vector3 position = Vector3.zero;

        position.x =maxX + paddingX;
        position.y =Random.Range(minY+paddingY,maxY-paddingY);

        return position;   
    }

    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)//敌人在右半边随机移动
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(middleX,maxY-paddingX);
        position.y = Random.Range(minY+paddingY,maxY-paddingY);

        return position;
    }
    
    public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)//敌人随机移动
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(minX+paddingX,maxY-paddingX);
        position.y = Random.Range(minY+paddingY,maxY-paddingY);

        return position;
    }

}
