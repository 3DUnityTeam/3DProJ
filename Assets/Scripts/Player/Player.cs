using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public enum SceneSetting
    {
        title, game
    };
    Rigidbody rigid;
    Animator myanim;
    private Transform myTR;

    public SceneSetting playerscene = SceneSetting.game;
    //Status
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
    //public GameManager manager;

    [Header("Mine")]
    public GameObject tofu;
    public GameObject CommonParent;
    public GameObject SpecialParent;
    //Damage
    [Header("#Damage")]
    bool isBlock = false;
    float blockTime = 0.2f;
    public float blockTimer = 0f;



    //이동
    [Header("Move")]
    public float moveSpeed = 5.0f;
    private float speed = 6.0f;
    private float v;
    private float h;
    Vector3 moveDir;
    private bool ismove;    
    private bool inputMove;
    private bool wallcollision; //벽과 닿은상태
    private float regenAPTime = 0;
    private bool regenAPStart = false;

    //카메라 회전
    [Header("Rotaion")]
    private float xTurnSpeed = 250f;
    public float XTurnSpeed { get { return this.xTurnSpeed; } set { this.xTurnSpeed = value; } }
    private float yTurnSpeed = 50f;
    public float YTurnSpeed { get { return this.yTurnSpeed; } set { this.yTurnSpeed = value; } }
    private float mousespeed;
    private bool inputRotate;
    float accumulatedInput = 0f;

    //대쉬
    [Header("Dash")]
    private bool isdashed = false;
    //대쉬 힘
    public float dashpower = 2500f;
    //대쉬 방향
    private Vector3 dashvector;
    //대쉬쿨
    public float dashcooltime = 0.3f;
    //대쉬 쿨 지난 시간
    private float dcooltime = 0f;
    //대쉬 쉬는 시간
    private float dashTime = 0;

    //점프
    [Header("Jump")]
    public bool isJump;
    public float JumpPower = 30.0f;
    //부스터
    [Header("Boost")]
    private bool isboost = false;
    public GameObject boost;
    public ParticleSystem boosterimpact;
    public GameObject[] boosters;
    //대쉬하다 점프할때 쓰는 변수
    private bool dashjumped;

    int movedirection;


    private void Awake()
    {
        dcooltime = dashcooltime;
        isJump = false;
        speed = moveSpeed;
        rigid = GetComponent<Rigidbody>();
        myTR = GetComponent<Transform>();
        myanim = tofu.GetComponent<Animator>();
    }
    // Update is called once per frame

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        //대쉬 상태 아닐때
        if (!isdashed)
        {
            //바뀐 화면 회전 속도 조정
            mousespeed = xTurnSpeed;
            //키입력

            if(playerscene == SceneSetting.game)
            {
                h = Input.GetAxisRaw("Horizontal");
                v = Input.GetAxisRaw("Vertical");
            }
            else if(playerscene == SceneSetting.title)
            {
                h = -(Input.GetAxisRaw("Horizontal"));
                v = -(Input.GetAxisRaw("Vertical"));
            }
            
            inputMove = (h != 0f || v != 0f);
            //이동은 fixedupdate
        }
        //대쉬 상태일때
        else if (isdashed)
        {
            //마우스 회전 속도 줄이기
            mousespeed = xTurnSpeed / 10;
        }

        //회전 입력
        if (GameManager.instance.isCursorLocked)
        {
            float r = Input.GetAxisRaw("Mouse X"); 
            accumulatedInput += r;
            inputRotate = (accumulatedInput != 0f);
            //회전은 fixedupdate
        }
        else
            inputRotate = false;

        //점프 부스트
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //점프 시작지점
            if (!isJump)
            {
                if(Input.GetKey(KeyCode.LeftShift))
                { dashjumped = true; }
                isJump = true;
                GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
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
            if(isdashed)
            {
                if(dashjumped)
                {
                    dashjumped = false;
                    rigid.useGravity = true;
                }
                else
                {
                    rigid.useGravity = false;
                }
            }
            else
            {
                rigid.useGravity = true;
            }
            

            isboost = false;
        }

        //대쉬
        if (Input.GetKeyDown(KeyCode.LeftShift) && ismove && dcooltime <= 0 && AP > 0)
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
            GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
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

        
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0 || HP <= 0)
        {
            return;
        }
        //대쉬 쿨타임
        dcooltime -= Time.fixedDeltaTime;
        //이동
        moveDir = (Vector3.forward * v) + (Vector3.right * h);
        Vector3 rayVec = (myTR.forward * v) + (myTR.right * h);
        Debug.DrawRay(myTR.position + new Vector3(0, 1.0f, 0) - rayVec.normalized, rayVec.normalized * (0.6f + rayVec.magnitude), Color.red);
        wallcollision = Physics.Raycast(myTR.position + new Vector3(0, 1.0f, 0)- rayVec, rayVec, 0.6f+rayVec.magnitude, LayerMask.GetMask("Wall"));
        if (inputMove)
        {
            if (!wallcollision)
            {
                myTR.Translate(moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
                ///
                /// 1. rigidbody.velocity : addforce 무시해서 사용안됨
                /// 2. rigidbody.addforce : 가속도 붙어서 사용안됨
                /// 3. rigidbody.MovePosition : 점프 부스트의 Transform 이동으로 씹힘
                ///
            }
        }

        //회전
        if (inputRotate)
        {
            float rot= mousespeed * Time.fixedDeltaTime * accumulatedInput;
            //rigid.rotation= Quaternion.AngleAxis(rot, Vector3.up) * rigid.rotation; // 좌우 회전
            myTR.Rotate(rot * Vector3.up);
            accumulatedInput = 0;
        }

        

        //애니메이션 작동(전방 후방 좌우)
        PlayerAnim(h, v);

        
        //빠르게 떨어지기
        if (!isdashed)
        {
            rigid.AddForce(Vector3.down * 500f);
        }

        //스태미나
        if (!isboost && !isdashed)
        {
            //부스트,점부 상태 확인용
            dashTime = 0;
            //스태미나 회복
            regenAPTime += Time.fixedDeltaTime;
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
            dashTime += Time.fixedDeltaTime;
            //1초마다
            if (dashTime > 1)
            {
                AP = AP - 8;
                dashTime = 0;
            }
        }
        //AP 스태미나 회복
        if (regenAPStart && AP <= 200)
        {
            AP = AP + (10 * Time.fixedDeltaTime);
            if (AP > 200)
            {
                AP = 200;
                regenAPStart = false;
            }
        }
    }

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
            moveSpeed = moveSpeed * 3.5f;
        }
        else
        {
            rigid.velocity = Vector3.zero;
            myanim.SetInteger("dashing", 2);
        }

    }

    public void GetHitDamage(float damage)
    {
        //dot
        if (damage < 5f)
        {
            HP = HP - damage;
        }
        //unit
        else
        {
            if (!isBlock)
            {
                isBlock = true;
                HP = HP - damage;
                StartCoroutine(BlockPile());
            }
        }
    }
    IEnumerator BlockPile()
    {
        if (isBlock)
        {
            while (blockTimer < blockTime)
            {
                yield return new WaitForFixedUpdate();
                blockTimer += Time.fixedDeltaTime;
            }
            isBlock = false;
            blockTimer = 0;
        }
        
    }
    private void OnEnable()
    {
        if(playerscene == SceneSetting.game)
        { GameManager.instance.AudioManager.PlaySfxLoop(AudioManager.Sfx.BoostLoop); }
        
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        HP = MaxHP;
        isBlock = false;
        blockTimer = 0;
    }
    //충돌 이벤트
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            isJump = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bomb"))
        {
            Blueberry berry;
            if(other.gameObject.TryGetComponent<Blueberry>(out berry))
            {
                if (!berry.bomb)
                {
                    berry.bomb = true;
                    GetHitDamage(berry.damage);
                }
            }

            SparrowBomb sparrow;
            if (other.gameObject.TryGetComponent<SparrowBomb>(out sparrow))
            {
                if (!sparrow.bomb)
                {
                    sparrow.bomb = true;
                    GetHitDamage(sparrow.damage);
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        FlameShot shot;
        if (other.TryGetComponent<FlameShot>(out shot))
        {
            GetHitDamage(shot.flameDmg * Time.fixedDeltaTime);
        }
        RollingFire rolling;
        if (other.TryGetComponent<RollingFire>(out rolling))
        {
            GetHitDamage(rolling.fireDmg * Time.fixedDeltaTime);
        }
    }
}
