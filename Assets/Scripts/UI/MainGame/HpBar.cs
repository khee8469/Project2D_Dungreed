using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : InGameUI
{
    float curHealth; //* ���� ü��
    public float maxHealth; //* �ִ� ü��
    public Slider HpBarSlider;
    public Text text;


    public void SetHp(int hp, int max) //���ξ����� ����
    {
        maxHealth = max;
        curHealth = hp;
        CheckHp();
        text.text = $"{curHealth.ToString()}/{maxHealth.ToString()}";
    }


    public void Damage(int hp) //* ������ �޴� �Լ�
    {
        curHealth = hp;
        CheckHp(); //* ü�� ����
        
        if (curHealth <= 0)
        {
            curHealth = 0;
        }
        text.text = $"{curHealth.ToString()}/{maxHealth.ToString()}";
    }
    public void CheckHp() //*HP ����
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }
}
