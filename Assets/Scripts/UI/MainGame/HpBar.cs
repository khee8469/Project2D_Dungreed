using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : InGameUI
{
    float curHealth; //* 현재 체력
    public float maxHealth; //* 최대 체력
    public Slider HpBarSlider;
    public Text text;


    public void SetHp(int hp, int max) //메인씬에서 실행
    {
        maxHealth = max;
        curHealth = hp;
        CheckHp();
        text.text = $"{curHealth.ToString()}/{maxHealth.ToString()}";
    }


    public void Damage(int hp) //* 데미지 받는 함수
    {
        curHealth = hp;
        CheckHp(); //* 체력 갱신
        
        if (curHealth <= 0)
        {
            curHealth = 0;
        }
        text.text = $"{curHealth.ToString()}/{maxHealth.ToString()}";
    }
    public void CheckHp() //*HP 갱신
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }
}
