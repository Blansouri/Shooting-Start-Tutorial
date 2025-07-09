using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookItem : MonoBehaviour
{
    protected Player player;

    [SerializeField] float minSpeed = 5f;

    [SerializeField] float maxSpeed = 15f;

    [SerializeField] protected AudioData defaultPickUpSFX;

    Animator animator;

    int pickUpStateID = Animator.StringToHash("PickUp");

    protected AudioData pickUpSFX;

    protected Text lootMessage;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        lootMessage = GetComponentInChildren<Text>(true);
        pickUpSFX = defaultPickUpSFX;
    }

    void OnEnable()
    {
        StartCoroutine(MoveCoroutine());  
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp();
    }

    protected virtual void PickUp()
    {
        StopAllCoroutines();
        animator.Play(pickUpStateID);
        AudioManager.Instance.PlayRandomSFX(pickUpSFX);
    }

    IEnumerator MoveCoroutine()
    {
        float speed = Random.Range(minSpeed,maxSpeed);

        Vector3 direction = Vector3.left ;

        while (true)
        {
            if (player.isActiveAndEnabled)
            {
                direction = (player.transform.position - transform.position).normalized;
            }

            transform.Translate(direction*speed*Time.deltaTime);

            yield return null;
        }
    }
}
