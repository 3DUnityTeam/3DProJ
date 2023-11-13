using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float effectTime;
    private SphereCollider myCD;
    // Start is called before the first frame update

    private void OnEnable()
    {
        StartCoroutine(Effectcontrol());
        myCD = GetComponent<SphereCollider>();
        myCD.radius = 0.1f;

    }
    // Update is called once per frame
    void Update()
    {
        while(myCD.radius <= 2.1)
        {
            myCD.radius = myCD.radius + Time.deltaTime;
        }
    }

    IEnumerator Effectcontrol()
    {
        yield return new WaitForSeconds(effectTime);
        gameObject.SetActive(false);
    }
}
