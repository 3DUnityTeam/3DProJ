using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaged : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.gameObject.GetComponent<TriggerCollison>() != null)
        {
            TriggerCollison trigger = other.gameObject.GetComponent<TriggerCollison>();
            GameObject effect = GameManager.instance.effectPoolManger.Get(trigger.effectcode - 1);
            effect.transform.position = other.ClosestPoint(transform.position);
        }
    }
}
