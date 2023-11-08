using System.Collections;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Transform firePoz;
    public GameObject bomb;

    Transform playerTrans_;
    Transform trans_;
    Animator ani_;

    [SerializeField]
    int maxHp = 1;
    int hp = 0;

    float traceDist = 3.0f;
    float speed = 4.0f;
    float rollingTime = 3.0f;

    Vector3 dirr = Vector3.zero;

    bool isTrace = false;
    bool flag = false;
    bool expFlag = false;

    private void Awake()
    {
        playerTrans_ = GameObject.FindWithTag("Player").GetComponent<Transform>();
        trans_ = GetComponent<Transform>();
        ani_ = GetComponent<Animator>();
        traceDist *= 3.5f;
    }

    private void Update()
    {
        if(hp < maxHp)
        {
            StartCoroutine("CheckState");
            StartCoroutine("DoMove");
        }
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
            }
            else
            {
                trans_.LookAt(playerTrans_);
                trans_.Translate(dirr * speed * 2f* Time.deltaTime);
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
                yield return new WaitForSeconds(5.0f);
            }
            else
            {
                dirr = Vector3.forward;
                yield return new WaitForSeconds(rollingTime);
                dirr = Vector3.zero;
            }
            flag = false;
        }
    }

    IEnumerator CheckState()
    {
        float dist = Vector3.Distance(playerTrans_.position, trans_.position);

        if (dist <= traceDist)
        {
            isTrace = true;
        }
        else
        {
            isTrace = false;
        }

        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator Explore()
    {
        if (!expFlag)
        {
            dirr = Vector3.zero;
            expFlag = true;
            ani_.SetBool("Atk", true);
            yield return new WaitForSeconds(2.0f);
            Instantiate(bomb, firePoz.position, firePoz.rotation);
            ani_.SetTrigger("Happy");
            yield return new WaitForSeconds(2.5f);
            Destroy(this.gameObject);
        }
    }

    void IsDead()
    {
        StartCoroutine("Explore");
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