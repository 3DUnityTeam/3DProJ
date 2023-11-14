using System.Collections;
using UnityEngine;

public class FishController : MonoBehaviour
{
    Transform playerTrans_;
    Transform trans_;
    Animator ani_;

    public GameObject[] lods;
    public GameObject fx;

    [SerializeField]
    int maxHp = 3;
    int hp = 0;

    [SerializeField]
    float traceDist = 6.0f;
    [SerializeField]
    float speed = 1.0f;

    Vector3 dirr = Vector3.zero;
    Vector3 poz = Vector3.zero;

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
        if (hp >= maxHp)
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
