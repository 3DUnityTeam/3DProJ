using System.Collections;
using UnityEngine;

public class BirdController : MobParent
{
    public Transform firePoz;
    public GameObject bomb;

    public GameObject[] lods;
    public GameObject fx;

    Transform playerTrans_;
    Transform trans_;
    Animator ani_;

    [SerializeField]
    float traceDist = 3.0f;
    [SerializeField]
    float speed = 4.0f;
    float rollingTime = 3.0f;

    Vector3 dirr = Vector3.zero;

    bool isTrace = false;
    bool flag = false;
    bool expFlag = false;
    bool spawn = true;  

    private void Awake()
    {
        MaxHP= 1;
        HP = 0;
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
        if(HP < MaxHP)
        {
            StartCoroutine("CheckState");
            StartCoroutine("DoMove");
        }
    }

    private void FixedUpdate()
    {
        if (HP >= MaxHP)
        {
            DeleteDict();
            IsDead();
        }
        else
        {
            if (spawn)
                dirr = Vector3.zero;
            else
            {
                if (!isTrace)
                {
                    trans_.Translate(dirr * speed * Time.fixedDeltaTime);
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
}
