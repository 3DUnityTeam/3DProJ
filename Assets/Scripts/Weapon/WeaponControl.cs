using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public enum State
    {
        normal,overheat,shooted,cooling
    };
    public State state = State.normal;
    [Header ("스테이터스")]
    public float Heat;
    public float currentheat = 0;
    public float HeatCooltime;
    private float cooltime;
    public float heatIncrease;
    public float heatDecrease;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.normal:
                currentheat = Mathf.Clamp(currentheat - heatDecrease * Time.deltaTime, 0f, float.MaxValue);
                
                if(currentheat >= Heat)
                {
                    StartCoroutine(CoolHeat());
                }
                break;
            case State.shooted:
                break;
            case State.overheat:
                currentheat = Heat;
                break;
            case State.cooling:
                currentheat -= heatDecrease * Time.deltaTime;

                if (currentheat <= 0)
                {
                    state = State.normal;
                }
                break;
        }
    }

    public IEnumerator Shoot()
    {
        currentheat += heatIncrease;
        if (currentheat >= Heat)
        {
            state = State.overheat;
            StartCoroutine(CoolHeat());
        }
        if (state == State.normal)
        {
            state = State.shooted;
            yield return new WaitForSeconds(HeatCooltime);
            state = State.normal;
        }
    }

    IEnumerator CoolHeat()
    {
        state = State.overheat;
        yield return new WaitForSeconds(HeatCooltime * 4);
        state = State.cooling;
    }
}
