using System.Collections;
using UnityEngine;

public class Blueberry : MonoBehaviour
{
    public GameObject fx;
    public float damage=12;

    public bool bomb;
    SphereCollider coll;
    float baseRadius;
    private void Awake()
    {
        bomb = false;
        coll = GetComponent<SphereCollider>();
        baseRadius = coll.radius;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(EnableFX());
    }

    IEnumerator EnableFX()
    {
        if (fx == null)
        {
            yield return null;  
        }
        fx.SetActive(true);
        while (coll.radius<=1.4f)   
        {
            yield return new WaitForFixedUpdate();
            coll.radius += ((1.4f - baseRadius) * Time.fixedDeltaTime)*0.5f;
        }
        coll.radius= baseRadius;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Poz"))
        {
            Destroy(this.gameObject, 1.2f);
            StartCoroutine(EnableFX());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            Destroy(this.gameObject, 1.2f);
            StartCoroutine(EnableFX());
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject, 1.2f);
            StartCoroutine(EnableFX());
        }
    }
}