using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    Rigidbody rigid;
    bool isJump;
    public int itemCount;
    AudioSource audio;
    //public GameManager manager;
    public GameObject tofu;
    public GameObject boost;
    public ParticleSystem boosterimpact;
    public GameObject[] boosters;

    public Animator myanim;
    private Transform myTR;
    private Vector3 dashvector;
    public float moveSpeed = 5.0f;
    private float speed = 6.0f;
    public float turnSpeed = 3000.0f;
    public float JumpPower = 30.0f;
    public float dashpower = 2500f;
    public float dashcooltime = 0.3f;

    private bool isdashed = false;
    private bool isboost = false;
    private float v;
    private float h;
    private float mousespeed;
    private bool ismove;
    private float dcooltime = 0f;
    int movedirection;


    private void Awake()
    {
        dcooltime = dashcooltime;
        Cursor.visible = false;
        isJump = false;
        speed = moveSpeed;
        rigid = GetComponent<Rigidbody>();
        myTR = GetComponent<Transform>();
        audio = GetComponent<AudioSource>();
        myanim = tofu.GetComponent<Animator>();
    }

    IEnumerator Start()
    {
        //myanim.Play("Idle");
        turnSpeed = 0.0f;
        yield return new WaitForSeconds(1.0f);
        turnSpeed = 3000.0f;
    }

    // Update is called once per frame
    void Update()
    {
        dcooltime -= Time.deltaTime;
        if (!isdashed)
        {

            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }

        if (!isdashed)
        {
            mousespeed = turnSpeed;
        }
        else if(isdashed)
        {
            mousespeed = turnSpeed / 10;
        }
            
        float r = Input.GetAxisRaw("Mouse X");

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!isJump)
            {
                isJump = true;
                boosterimpact.Play();
                rigid.velocity = new Vector3(0, JumpPower, 0);
                isboost = true;
            }
            else if(isJump)
            {
                isboost = true;
            } 
        }
        if (isboost)
        {
            boost.SetActive(true);
            rigid.useGravity = false;
            myTR.position += Vector3.up * 0.06f;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            boost.SetActive(false);
            if(!isdashed)
            {
                rigid.useGravity = true;
            }
            isboost = false;
        }


        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        if(isdashed)
        { 
        }
        myTR.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        myTR.Rotate(Vector3.up * mousespeed * Time.deltaTime * r);

        if (Input.GetKeyDown(KeyCode.LeftShift) && ismove && dcooltime <= 0)
        {
            // 현재 방향을 기준으로 하는 벡터를 만듭니다.
            rigid.velocity = Vector3.zero;
            if(!isdashed)
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
                /*if (Input.GetKey(KeyCode.W))
                {
                    movedirection = 0;
                    dashvector = myTR.forward;
                    myanim.SetInteger("dashtype", 0);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    movedirection = 0;
                    dashvector = -myTR.forward;
                    myanim.SetInteger("dashtype", 1);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    movedirection = 1;
                    dashvector = myTR.right;
                    myanim.SetInteger("dashtype", 2);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    movedirection = 1;
                    dashvector = -myTR.right;
                    myanim.SetInteger("dashtype", 3);
                }*/
            }
            
            Vector3 forceDirection = dashvector * dashpower;

            // AddForce에 현재 방향을 기준으로 하는 힘을 가합니다.
            
            StartCoroutine(Dashjinheng(0.22f));
            myanim.SetTrigger("dash");
            myanim.SetInteger("dashing", 0);
            boosterimpact.Play();
            rigid.AddForce(forceDirection , ForceMode.Impulse);
            dcooltime = dashcooltime;


            IEnumerator Dashjinheng(float time)
            {
                yield return new WaitForSeconds(time);

                if(Input.GetKey(KeyCode.LeftShift) && !isdashed)
                {
                    myanim.SetInteger("dashing", 1);
                    rigid.velocity = Vector3.zero;
                    isdashed = true;
                    switch(movedirection)
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


    }

    private void FixedUpdate()
    {
        if(!isdashed)
        {
            rigid.AddForce(Vector3.down * 500f);
        }
            
    }

    void PlayerAnim(float h, float v)
    {
        
        if(v>=0.1f)
        {
            ismove = true;
            if (!isdashed)
            {
                boosters[0].SetActive(true);
                boosters[1].SetActive(true);
                if(h >= 0.1f)
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
        else if( v <= -0.1f)
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
                if(v <= -0.1f)
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
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
        }
    }

    
}
