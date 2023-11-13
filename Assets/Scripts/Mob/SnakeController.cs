using System.Collections;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    enum State
    {
        IDLE, TRACE
    }

    State state = State.IDLE;

    Transform playerTrans_;
    Transform trans_;
    Animator ani_;

    public GameObject fx;
    public GameObject[] lods;

    [SerializeField]
    int maxHp = 2;
    int hp = 0;

    [SerializeField]
    float traceDist = 50f;
    float speed = 3.0f;
    float rollingTime = 5.0f;

    Vector3 dirr = Vector3.zero;

    bool flag = false;
    bool atk = false;
    bool spawn = true;

    private void Awake()
    {
        Fx(false);
        playerTrans_ = GameObject.FindWithTag("Player").GetComponent<Transform>();
        trans_ = GetComponent<Transform>();
        ani_ = GetComponent<Animator>();
        traceDist *= 3.5f;
    }

    void Fx(bool t)
    {
        for(int i = 0; i < lods.Length; i++)
        {
            lods[i].SetActive(t);
        }
    }

    private IEnumerator Start()
    {
        fx.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(true);
        Fx(true);
        fx.SetActive(false);
        spawn = false;
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
            {
                dirr = Vector3.zero;
            }
            else
            {
                if (state == State.IDLE)
                {
                    trans_.Translate(dirr * speed * Time.fixedDeltaTime);
                    ani_.SetBool("Spin", false);
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
            if(state == State.IDLE)
            {
                atk = false;
                trans_.Rotate(0, Random.Range(0, 180f), 0);
                dirr = Vector3.forward;
                yield return new WaitForSeconds(3.0f);
            }
            else
            {
                trans_.LookAt(playerTrans_);
                dirr = Vector3.forward;
                atk = true;
                yield return new WaitForSeconds(rollingTime);
                dirr = Vector3.zero;
            }
            flag = false;
        }
    }

    IEnumerator CheckState()
    {
        float dist = Vector3.Distance(playerTrans_.position, trans_.position);

        if(dist <= traceDist)
        {
            //Debug.Log("I found player");
            state = State.TRACE;
        }
        else
        {
            state = State.IDLE;
        }

        yield return new WaitForSeconds(0.3f);
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
            ani_.SetTrigger("Hit");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            
        }
    }
}
