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

            if (state == State.IDLE)
            {
                trans_.Translate(dirr * speed * Time.deltaTime);
                ani_.SetBool("Spin", false);
            }
            else
            {
                //trans_.Translate(dirr * speed * 2.5f* Time.deltaTime);
                ani_.SetBool("Spin", true);
                trans_.Translate(dirr * speed * 2.5f * Time.deltaTime);
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
