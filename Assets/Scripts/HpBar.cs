using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    protected float curHealth; //* 현재 체력
    public float maxHealth; //* 최대 체력
    public Slider HpBarSlider;
    [SerializeField] private Image barImage;

    public void SetHp(float amount) //*Hp설정
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }

   

    public void CheckHp() //*HP 갱신
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }

    public void Damage(float damage) //* 데미지 받는 함수
    {
        if (maxHealth == 0 || curHealth <= 0) //* 이미 체력 0이하면 패스
            return;
        curHealth -= damage;
        CheckHp(); //* 체력 갱신
        if (curHealth <= 0)
        {
            //* 체력이 0 이하라 죽음
        }
    }

    

    private void ChangeHealthBarAmount(float amount) //* HP 게이지 변경 
    {
        barImage.fillAmount = amount;

        //* HP가 0이거나 꽉차면 HP바 숨기기
        if (barImage.fillAmount == 0f || barImage.fillAmount == 1f)
        {
            Hide();
        }
    }
}
