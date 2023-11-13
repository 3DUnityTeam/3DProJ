using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectControl : MonoBehaviour
{
    public float effectTime;
    private void OnEnable()
    {
        StartCoroutine(Effectcontrol());
    }

    IEnumerator Effectcontrol()
    {
        yield return new WaitForSeconds(effectTime);
        gameObject.SetActive(false);
    }

}
