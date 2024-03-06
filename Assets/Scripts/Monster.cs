using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum State { Idle, Trace, Attack, Die }
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int hp;

    private State state = State.Idle;
    

    public void HitDamage(int damage)
    {
        Debug.Log("데미지");
        hp -= damage;
    }
    private void Die()
    {
        if(hp <= 0)
        {
            Debug.Log("사망");
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        Die();

        switch (state)
        {
            case State.Idle:

                break; 
            case State.Trace:

                break;
            case State.Attack:

                break;
            case State.Die:

                break;
        }
    }

    public void ChangeState(State state)
    {
        this.state = state;
    }

}
