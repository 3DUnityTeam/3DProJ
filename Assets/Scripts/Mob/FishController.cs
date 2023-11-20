using System.Collections;
using UnityEngine;

public class FishController : MobParent
{
    public GameObject heart;
    Transform playerTrans_;
    Transform trans_;
    Animator ani_;

    public GameObject[] lods;
    public GameObject fx;

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
        MaxHP = 120;
        mobClear = true;
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

    public override void OnEnable()
    {
        isTrace = false;
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
        Fx(false);
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(true);
        Fx(true);
        spawn = false;
        fx.SetActive(false);

        //���� ���ӵ�����
        StartCoroutine(IsLive(15));
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
                    if (dit <= 5)
                    {
                        trans_.Translate(dirr * 0 * Time.fixedDeltaTime);

                    }
                    else
                    {
                        trans_.Translate(dirr * speed * Time.fixedDeltaTime);
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
                
                yield return new WaitForSeconds(3.3f);
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
                yield return new WaitForSeconds(3);
            }
            else
            {
                isTrace = false;
                yield return new WaitForSeconds(0.3f);
            }
            checkFlag = false;
        }

    }

    public override void IsDead()
    {
        heart.SetActive(true);
        ani_.SetTrigger("Happy");
        base.IsDead();
    }
}
