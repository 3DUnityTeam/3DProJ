using System.Collections;
using UnityEngine;

public class CoffeeController : MobParent
{
    public GameObject heart;
    public GameObject coffee;
    public GameObject warnCoffee;
    
    public GameObject[] lods;
    public GameObject fx;

    Transform playerTrans_;
    Transform trans_;
    Animator ani_;

    [SerializeField]
    float traceDist = 3.0f;
    [SerializeField]
    float speed = 1.0f;

    Vector3 dirr = Vector3.zero;

    bool isTrace = false;
    bool flag = false;
    bool checkFlag = false;
    bool spawn = true;

    private void Awake()
    {
        MaxHP = 200;
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
        
        //모판 지속데미지
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
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                ani_.SetBool("Atk", true);
                Vector3 firePoz = playerTrans_.position + new Vector3(0, 5, 0);
                Instantiate(warnCoffee, firePoz, playerTrans_.rotation);
                GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.ObjSpawn);
                yield return new WaitForSeconds(0.3f);
                ani_.SetBool("Atk", false);
                firePoz += new Vector3(0, 10, 0);
                Instantiate(coffee, firePoz, playerTrans_.rotation);
                yield return new WaitForSeconds(1.8f);
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
                yield return new WaitForSeconds(2.5f);
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
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Happy);
        ani_.SetTrigger("Happy");
        base.IsDead();
    }
}
