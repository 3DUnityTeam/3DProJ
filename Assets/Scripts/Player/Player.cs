using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    Animator myanim;
    private Transform myTR;

    public int itemCount;
    //public GameManager manager;
    [Header("Mine")]
    public GameObject tofu;
    //HP
    private float maxHP = 100f;
    public float MaxHP { get { return this.maxHP; } }
    private float hp = 100f;
    public float HP { get { return this.hp; } set { this.hp = value; } }
    //AP
    private float maxAP = 200f;
    public float MaxAP { get { return this.maxAP; } }
    public float ap = 200f;
    public float AP { get { return this.ap; } set { this.ap = value; } }

    //이동
    [Header("Move")]
    public float moveSpeed = 5.0f;
    private float speed = 6.0f;
    private float v;
    private float h;
    private bool ismove;
    //회전
    [Header("Rotaion")]
    public float turnSpeed = 3000.0f;
    private float mousespeed;
    //점프
    [Header("Jump")]
    public bool isJump;
    public float JumpPower = 30.0f;
    //부스터
    [Header("Boost")]
    public GameObject boost;
    public ParticleSystem boosterimpact;
    public float dashpower = 2500f;
    public float dashcooltime = 0.3f;
    public GameObject[] boosters;
    private Vector3 dashvector;
    private bool isdashed = false;
    private bool isboost = false;
    private float dcooltime = 0f;
    private float dashTime = 0;
    private float regenAPTime = 0;
    private bool regenAPStart = false;
    int movedirection;

    private void Awake()
    {
        dcooltime = dashcooltime;
        //Cursor.visible = false;
        isJump = false;
        speed = moveSpeed;
        rigid = GetComponent<Rigidbody>();
        myTR = GetComponent<Transform>();
        myanim = tofu.GetComponent<Animator>();
    }

    IEnumerator Start()
    {
        turnSpeed = 0.0f;
        yield return new WaitForSeconds(1.0f);
        turnSpeed = 3000.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale==0 || HP<=0)
        {
            return;
        }
        //대쉬 쿨타임
        dcooltime -= Time.deltaTime;
        //대쉬 상태 아닐때
        if (!isdashed)
        {
            //바뀐 화면 회전 속도 조정
            mousespeed = turnSpeed; 
            //키입력
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }
        //대쉬 상태일때
        else if (isdashed)
        {
            //마우스 회전 속도 줄이기
            mousespeed = turnSpeed / 10;
        }
        //이동
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        myTR.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        //회전
        float r = Input.GetAxisRaw("Mouse X");
        myTR.Rotate(Vector3.up * mousespeed * Time.deltaTime * r);

        //점프 부스트
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //점프 시작지점
            if (!isJump)
            {
                isJump = true;
                boosterimpact.Play();
                rigid.velocity = new Vector3(0, JumpPower, 0);
                isboost = true;
            }
            //점프 도중 부스트
            else if (isJump)
            {
                isboost = true;
            }
        }
        //점프 부스트 상태일때
        if (isboost)
        {
            boost.SetActive(true);
            rigid.useGravity = false;
            myTR.position += Vector3.up * 0.06f;
        }
        //점프 부스트 종료
        if (Input.GetKeyUp(KeyCode.Space))
        {
            boost.SetActive(false);
            rigid.useGravity = true;
            isboost = false;
        }
        //부스트
        if (Input.GetKeyDown(KeyCode.LeftShift) && ismove && dcooltime <= 0 && AP>0)
        {
            // 현재 방향을 기준으로 하는 벡터를 만듭니다.
            rigid.velocity = Vector3.zero;
            //대쉬 방향
            if (!isdashed)
            {
                if (v >= 0.1f)
                {
                    movedirection = 0;
                    dashvector = myTR.forward;
                }
                else if (v <= -0.1f)
                {
                    movedirection = 0;
                    dashvector = -myTR.forward;
                }
                else if (h >= 0.1f)
                {
                    movedirection = 1;
                    dashvector = myTR.right;
                }
                else if (h <= -0.1f)
                {
                    movedirection = 1;
                    dashvector = -myTR.right;
                }
                else
                {
                    movedirection = 0;
                    dashvector = myTR.right * 0;
                }
            }
            //대쉬 방향 및 힘
            Vector3 forceDirection = dashvector * dashpower;

            //코루틴작동
            StartCoroutine(Dashjinheng(0.22f));
            //이펙트 작동
            myanim.SetTrigger("dash");
            myanim.SetInteger("dashing", 0);
            boosterimpact.Play();
            // AddForce에 현재 방향을 기준으로 하는 힘을 가합니다.
            rigid.AddForce(forceDirection, ForceMode.Impulse);
            //쿨타임
            dcooltime = dashcooltime;
            //스태미나 소모
            AP = AP - 50;
            regenAPStart = false;
            regenAPTime = 0;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isdashed)
        {
            myanim.SetInteger("dashing", 2);
            isdashed = false;
            rigid.useGravity = true;
            rigid.velocity = Vector3.zero;
            moveSpeed = speed;
        }


        PlayerAnim(h, v);

        //스태미나
        if (!isboost && !isdashed)
        {
            //부스트,점부 상태 확인용
            dashTime = 0;
            //스태미나 회복
            regenAPTime += Time.deltaTime;
            if (!regenAPStart)
            {
                if (AP <= 0)
                {
                    if (regenAPTime >= 2)
                    {
                        regenAPStart = true;
                        regenAPTime = 0;
                    }
                }
                else
                {
                    if (regenAPTime >= 1)
                    {
                        regenAPStart = true;
                        regenAPTime = 0;
                    }
                }
            }
        }
        else
        {
            if (AP <= 0)
            {
                myanim.SetInteger("dashing", 2);
                isdashed = false;
                rigid.velocity = Vector3.zero;
                moveSpeed = speed;
                //점프 종료
                boost.SetActive(false);
                rigid.useGravity = true;
                isboost = false;
            }
            regenAPStart = false;
            regenAPTime = 0;
            //시간 측정
            dashTime += Time.deltaTime;
            //1초마다
            if (dashTime > 1)
            {
                AP = AP - 8;
                dashTime = 0;
            }
        }
        //AP 스태미나 회복
        if (regenAPStart && AP<=200)
        {
            AP = AP + (10 * Time.deltaTime);
            if (AP > 200)
            {
                AP = 200;
                regenAPStart = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isdashed)
        {
            rigid.AddForce(Vector3.down * 500f);
        }

    }
    //플레이어 이동 애니메이션
    void PlayerAnim(float h, float v)
    {

        if (v >= 0.1f)
        {
            ismove = true;
            if (!isdashed)
            {
                boosters[0].SetActive(true);
                boosters[1].SetActive(true);
                if (h >= 0.1f)
                {
                    boosters[2].SetActive(true);
                    boosters[3].SetActive(false);
                }
                else if (h <= -0.1f)
                {
                    boosters[3].SetActive(true);
                    boosters[2].SetActive(false);
                }
                else
                {
                    boosters[3].SetActive(false);
                    boosters[2].SetActive(false);
                }
            }

            myanim.SetInteger("dashtype", 0);
        }
        else if (v <= -0.1f)
        {
            ismove = true;
            if (!isdashed)
            {
                boosters[0].SetActive(false);
                boosters[1].SetActive(false);
                if (h >= 0.1f)
                {
                    boosters[2].SetActive(true);
                    boosters[3].SetActive(false);
                }
                else if (h <= -0.1f)
                {
                    boosters[3].SetActive(true);
                    boosters[2].SetActive(false);
                }
                else
                {
                    boosters[3].SetActive(false);
                    boosters[2].SetActive(false);
                }
            }
            myanim.SetInteger("dashtype", 1);
        }
        else if (h >= 0.1f)
        {
            ismove = true;
            if (!isdashed)
            {
                boosters[2].SetActive(true);
                boosters[3].SetActive(false);
                if (v <= -0.1f)
                {
                    myanim.SetInteger("dashtype", 1);
                }
            }
            myanim.SetInteger("dashtype", 2);
        }
        else if (h <= -0.1f)
        {

            ismove = true;
            if (!isdashed)
            {
                boosters[2].SetActive(false);
                boosters[3].SetActive(true);
                if (v <= -0.1f)
                {
                    myanim.SetInteger("dashtype", 1);
                }
            }
            myanim.SetInteger("dashtype", 3);
        }
        else
        {
            boosters[0].SetActive(false);
            boosters[1].SetActive(false);
            boosters[2].SetActive(false);
            boosters[3].SetActive(false);
            ismove = false;
            myanim.SetInteger("dashtype", -1);
        }
    }
    //플레이어 지속 부스트 애니메이션
    IEnumerator Dashjinheng(float time)
    {
        if (AP < 8)
        {
            //대쉬 초기화
            myanim.SetInteger("dashing", 2);
            isdashed = false;
            rigid.velocity = Vector3.zero;
            moveSpeed = speed;
            yield return null;
        }
        yield return new WaitForSeconds(time);

        if (Input.GetKey(KeyCode.LeftShift) && !isdashed)
        {
            myanim.SetInteger("dashing", 1);
            rigid.velocity = Vector3.zero;
            isdashed = true;
            switch (movedirection)
            {
                case 0:
                    h = 0f;
                    break;
                case 1:
                    v = 0f;
                    break;
            }
            rigid.useGravity = false;
            moveSpeed = moveSpeed * 6;
        }
        else
        {
            rigid.velocity = Vector3.zero;
            myanim.SetInteger("dashing", 2);
        }

    }
    //충돌 이벤트
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
        }
    }

}
