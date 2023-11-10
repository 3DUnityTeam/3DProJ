using System.Collections;
using UnityEngine;

public class CoffeeController : MonoBehaviour
{
    public GameObject coffee;
    public GameObject warnCoffee;

    Transform playerTrans_;
    Transform trans_;
    Animator ani_;

    [SerializeField]
    int maxHp = 5;
    int hp = 0;

    float traceDist = 3.0f;
    float speed = 1.0f;

    Vector3 dirr = Vector3.zero;

    bool isTrace = false;
    bool flag = false;
    bool checkFlag = false;

    private void Awake()
    {
        playerTrans_ = GameObject.FindWithTag("Player").GetComponent<Transform>();
        trans_ = GetComponent<Transform>();
        ani_ = GetComponent<Animator>();
        traceDist *= 3.5f;
    }

    private void Update()
    {
        StartCoroutine("CheckState");
        StartCoroutine("DoMove");
    }

    private void FixedUpdate()
    {
        if (hp == maxHp)
            IsDead();
        else
        {

            if (!isTrace)
            {
                trans_.Translate(dirr * speed * Time.deltaTime);
                ani_.SetBool("Atk", false);
            }
            else
            {
                //trans_.Translate(dirr * speed * 2.5f* Time.deltaTime);
                trans_.LookAt(playerTrans_);
            }
        }
    }
    IEnumerator DoMove()
    {
        if (!flag)
        {
            flag = true;
            if (!isTrace)
            {
                trans_.Rotate(0, Random.Range(0, 180f), 0);
                dirr = Vector3.forward;
                yield return new WaitForSeconds(6.5f);
            }
            else
            {
                ani_.SetBool("Atk", true);
                Vector3 firePoz = playerTrans_.position + new Vector3(0, 5, 0);
                Instantiate(warnCoffee, firePoz, playerTrans_.rotation);
                yield return new WaitForSeconds(0.8f);
                ani_.SetBool("Atk", false);
                firePoz += new Vector3(0, 5, 0);
                Instantiate(coffee, firePoz, playerTrans_.rotation);
                yield return new WaitForSeconds(8f);
            }
            flag = false;
        }
    }

    IEnumerator CheckState()
    {
        if (!checkFlag)
        {
            checkFlag = true;
            float dist = Vector3.Distance(playerTrans_.position, trans_.position);

            if (dist <= traceDist)
            {
                isTrace = true;
                yield return new WaitForSeconds(10f);
            }
            else
            {
                isTrace = false;
                yield return new WaitForSeconds(0.3f);
            }
            checkFlag = false;
        }
    }

    void IsDead()
    {
        ani_.SetTrigger("Happy");
        Destroy(this.gameObject, 2.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp++;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }
}
