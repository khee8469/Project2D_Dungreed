using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] float curHealth; //* ���� ü��
    //protected float curHealth; //* ���� ü��
    public float maxHealth; //* �ִ� ü��
    public Slider HpBarSlider;
    //[SerializeField] private Image barImage;

    private void Start()
    {
        SetHp();
    }

    public void SetHp() //*Hp����
    {
        Debug.Log(Manager.Game.player.GetHp());
        maxHealth = Manager.Game.player.GetHp();
        curHealth = maxHealth;
    }


    public void Damage(float damage) //* ������ �޴� �Լ�
    {
        if (maxHealth == 0 || curHealth <= 0) //* �̹� ü�� 0���ϸ� �н�
            return;
        curHealth -= damage;
        CheckHp(); //* ü�� ����
        if (curHealth <= 0)
        {
            //* ü���� 0 ���϶� ����
        }
    }
    public void CheckHp() //*HP ����
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }



    /*private void ChangeHealthBarAmount(float amount) //* HP ������ ���� 
    {
        barImage.fillAmount = amount;

        //* HP�� 0�̰ų� ������ HP�� �����
        if (barImage.fillAmount == 0f || barImage.fillAmount == 1f)
        {
            //Hide();
        }
    }*/



}
