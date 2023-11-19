using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misale : MonoBehaviour
{
    public int effectID = 1;
    public float damage = 20.0f;
    public GameObject boost;
    private AimManager aimManager;
    public float bulletspeed = 2000f;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        aimManager = GameManager.instance.AimManager;
    }

    void OnEnable()
    {
        boost.SetActive(false);
        StartCoroutine(ActiveFalse());
        StartCoroutine(Stop());
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mob") || collision.gameObject.CompareTag("Land"))
        {
            GameObject effect = GameManager.instance.effectPoolManger.Get(effectID - 1);

            if (effect.GetComponent<TriggerCollison>() != null)
            {
                effect.GetComponent<TriggerCollison>().damage = damage;
                damage = 0;
            }



            effect.transform.position = collision.contacts[0].point;
            gameObject.SetActive(false);
        }
    }

    IEnumerator ActiveFalse()
    {
        yield return new WaitForSeconds(30f);
        gameObject.SetActive(false);
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1f);
        if (aimManager.aimingTarget != null)
        {
            // 타겟을 향하도록 회전 설정
            boost.SetActive(true);
            Vector3 center = aimManager.aimingTarget.GetComponent<Collider>().bounds.center;
            transform.LookAt(center);
            Vector3 direction = (center - transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(direction * bulletspeed);
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(Vector3.down * bulletspeed);
        }
    }

}
