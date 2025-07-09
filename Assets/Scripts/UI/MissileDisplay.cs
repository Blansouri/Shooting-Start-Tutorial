using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    static Text amountText;

    static Image cooldownImage;

    void Awake()
    {
        cooldownImage = transform.Find("Cooldown Image").GetComponent<Image>();
        amountText = transform.Find("Amount Text").GetComponent<Text>();
    }

    void Start()
    {
        ScoreManager.Instance.ResetScore();

    }

    public static void UpdateAmountText(int amount) => amountText.text = amount.ToString();
    public static void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;//冷却图片填充
}
