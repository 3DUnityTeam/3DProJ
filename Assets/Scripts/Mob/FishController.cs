using System.Collections;
using UnityEngine;

public class FishController : MonoBehaviour
{
    Transform playerTrans_;
    Transform trans_;
    Animator ani_;

    [SerializeField]
    int maxHp = 3;
    int hp = 0;

    float traceDist = 6.0f;
    float speed = 1.0f;

    Vector3 dirr = Vector3.zero;
    Vector3 poz = Vector3.zero;

    bool isTrace = false;
    bool flag = false;
    bool checkFlag = false;
    bool isArrive = false;

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
                trans_.Translate(dirr * speed * Time.fixedDeltaTime);
                ani_.SetBool("Atk", false);
            }
            else
            {
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
                yield return new WaitForSeconds(1.8f);
                dirr = Vector3.zero;
            }
            else
            {
                CheckRange();
                yield return new WaitForSeconds(10.5f);
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
                yield return new WaitForSeconds(9);
            }
            else
            {
                isTrace = false;
                yield return new WaitForSeconds(0.3f);
            }
            checkFlag = false;
        }

    }


    void CheckRange()
    {
        poz = trans_.position;
        float x = poz.x;    float z = poz.z;
        x = Random.Range(x - 2.5f , x+ 2.5f );
        z = Random.Range(z - 2.5f, x + 2.5f );
        poz = new Vector3(x, 0, z);
        dirr = poz;
        Debug.Log(dirr);
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
