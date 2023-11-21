using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparrowBomb : MonoBehaviour
{
    SphereCollider coll;
    float baseRadius;
    float effectTime;

    public float damage = 30;
    public bool bomb = false;
    private void Awake()
    {
        baseRadius = 0;
        coll = GetComponent<SphereCollider>();
        effectTime = gameObject.GetComponent<ParticleSystem>().duration;
    }

    IEnumerator Start()
    {
        while (coll.radius <= 2f)
        {
            yield return new WaitForFixedUpdate();
            coll.radius += ((2f - baseRadius) * Time.fixedDeltaTime) * effectTime;
        }
        coll.radius = baseRadius;
    }
}
