using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour//制导系统
{
    [SerializeField] Projectile projectile;

    [SerializeField] float minBallisticAngle = 50f;//最小偏转角

    [SerializeField] float maxBallisticAngle = 75f;//最大偏转角

    float ballisticAngle;

    Vector3 targetDirection;

    public IEnumerator HomingCoroutine(GameObject target)
    {

        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);//随机偏转角

        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                targetDirection = target.transform.position - transform.position;

                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y,targetDirection.x)*Mathf.Rad2Deg,Vector3.forward);

                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);

                projectile.Move();
            }
            else
            {
                projectile.Move();
            }

            yield return null;

        }
        
    }
}
