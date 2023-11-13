using System.Collections;
using UnityEngine;

public class CoffeeController : MonoBehaviour
{
    public GameObject coffee;
    public GameObject warnCoffee;
    
    public GameObject[] lods;
    public GameObject fx;

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
    bool spawn = true;

    private void Awake()
    {
        playerTrans_ = GameObject.FindWithTag("Player").GetComponent<Transform>();
        trans_ = GetComponent<Transform>();
        ani_ = GetComponent<Animator>();
        traceDist *= 3.5f;
    }

    void Fx(bool t)
    {
        for (int i = 0; i < lods.Length; i++)
        {
            lods[i].SetActive(t);
        }
    }

    private IEnumerator Start()
    {
        fx.SetActive(true);
        Fx(false);
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(true);
        Fx(true);
        spawn = false;
        fx.SetActive(false);
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
            if (spawn)
                dirr = Vector3.zero;
            else
            {
                if (!isTrace)
                {
                    trans_.Translate(dirr * speed * Time.fixedDeltaTime);
                    ani_.SetBool("Atk", false);
                }
                else
                {
                    trans_.LookAt(new Vector3(playerTrans_.position.x, trans_.position.y, playerTrans_.position.z));
                    float dit = Vector3.Distance(playerTrans_.position, trans_.position);
                    if (dit >= 5)
                    {
                        trans_.Translate(dirr * speed * Time.fixedDeltaTime);
                    }
                    else
                    {
                        trans_.Translate(dirr * 0 * Time.fixedDeltaTime);
                    }
                }
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
