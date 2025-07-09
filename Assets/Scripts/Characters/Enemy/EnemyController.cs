using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("-MOVE-")]

    [SerializeField] protected float paddingX;

    [SerializeField] protected float paddingY;

    [SerializeField] float moveSpeed = 1.5f;

    [SerializeField] float moveRotationAngle = 25f;

    [Header("-FIRE-")]

    [SerializeField] protected float minFireInterval = 0.2f;
    
    [SerializeField] protected float maxFireInterval = 0.5f;

    [SerializeField] protected GameObject[] projectiles;

    [SerializeField] protected AudioData[] projectileLunchSFX;

    [SerializeField] protected Transform muzzle;

    protected Vector3 targetPosition;

    GameObject player;


    protected virtual void Awake()
    {
        var size =transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    private void Start()
    {
       
    }

    protected virtual void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    void OnDisable()
    {
        StopAllCoroutines();   
    }

    IEnumerator RandomlyMovingCoroutine()//敌人随机移动
    {
       transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

       targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            
            if (Vector3.Distance(targetPosition, transform.position) > 0.0001f)//现在位置和目标位置的差值大于一个极小值(Math.Epsition)时
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);//向目标位置移动

                transform.rotation = Quaternion.AngleAxis((targetPosition-transform.position).normalized.y*moveRotationAngle,Vector3.right);//在x轴转动角度
                //Debug.Log("position"+transform.position);
                //Debug.Log("Distance" + Vector3.Distance(targetPosition,transform.position));

            }
            else//在随机生成一个目标位置
            {
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
                //Debug.Log("targrtPosition:" + targetPosition);
            }
            yield return null;
        }
    }

    protected virtual IEnumerator RandomlyFireCoroutine()//随机开火
    {
        while (gameObject.activeSelf&&player.activeSelf)
        {
            if(GameManager.GameState == GameState.GameOver)yield break;
            if (player.transform.position.y - 0.21f < transform.position.y && transform.position.y < player.transform.position.y + 0.21f)
            {
                yield return new WaitForSeconds(minFireInterval);
                foreach (var projectile in projectiles)
                {
                    PoolManager.Release(projectile, muzzle.position);
                }


            }

            else
            {
                yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
                foreach (var projectile in projectiles)
                {
                    PoolManager.Release(projectile, muzzle.position);
                }
            }

            AudioManager.Instance.PlayRandomSFX(projectileLunchSFX);
        }
    }

}
