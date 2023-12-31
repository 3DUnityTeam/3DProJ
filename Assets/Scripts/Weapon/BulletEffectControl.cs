using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectControl : MonoBehaviour
{
    public AudioManager.Sfx sound;
    public float effectTime;
    public float lightTime;
    public GameObject effectlight;
    private void OnEnable()
    {
        GameManager.instance.AudioManager.PlaySfx(sound);
        StartCoroutine(Effectcontrol());

        if(effectlight != null)
        {
            StartCoroutine(LightControl());
        }
    }

    IEnumerator Effectcontrol()
    {
        yield return new WaitForSeconds(effectTime);
        gameObject.SetActive(false);
    }

    IEnumerator LightControl()
    {
        yield return new WaitForSeconds(lightTime);
        gameObject.SetActive(false);
    }

}
