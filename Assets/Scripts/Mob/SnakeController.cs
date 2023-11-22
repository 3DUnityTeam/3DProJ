using System.Collections;
using UnityEngine;

public class SnakeController : MobParent
{
    public GameObject heart;
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
    float traceDist = 50f;
    [SerializeField]
    float speed = 3.0f;
    float rollingTime = 5.0f;

    Vector3 dirr = Vector3.zero;

    bool flag = false;
    bool atk = false;
    bool spawn = true;
    private void Awake()
    {
        MaxHP = 80;
        Fx(false);
        mobClear = true;
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

    public override void OnEnable()
    {
        atk = false;
        flag = false;
        spawn = true;
        StartCoroutine(Start());
        base.OnEnable();
    }
    private new IEnumerator Start()
    {
        base.Start();
        playerTrans_ = GameManager.instance.player.transform;

        fx.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(true);
        Fx(true);
        fx.SetActive(false);
        spawn = false;

        //���� ���ӵ�����
        StartCoroutine(IsLive(10));
    }

    private void Update()
    {
        if (Dead)
            return;
        StartCoroutine("CheckState");
        StartCoroutine("DoMove");
    }

    private void FixedUpdate()
    {
        if (Dead)
            return;
        if (HP >= MaxHP)
        {
            IsDead();
        }
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
                        trans_.Translate(dirr * speed*1.2f * Time.fixedDeltaTime);
                    }
                    else
                    {
                        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Atk);
                        ani_.SetTrigger("Atk");
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
                ani_.SetBool("Spin", true);
                yield return new WaitForSeconds(rollingTime);
                ani_.SetBool("Spin", false);
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

    public override void IsDead()
    {
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Happy);
        heart.SetActive(true);
        ani_.SetTrigger("Happy");
        base.IsDead();
    }

}
