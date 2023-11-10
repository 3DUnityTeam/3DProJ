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

    //�̵�
    [Header("Move")]
    public float moveSpeed = 5.0f;
    private float speed = 6.0f;
    private float v;
    private float h;
    private bool ismove;
    //ȸ��
    [Header("Rotaion")]
    public float turnSpeed = 3000.0f;
    private float mousespeed;
    //����
    [Header("Jump")]
    public bool isJump;
    public float JumpPower = 30.0f;
    //�ν���
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
        //�뽬 ��Ÿ��
        dcooltime -= Time.deltaTime;
        //�뽬 ���� �ƴҶ�
        if (!isdashed)
        {
            //�ٲ� ȭ�� ȸ�� �ӵ� ����
            mousespeed = turnSpeed; 
            //Ű�Է�
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }
        //�뽬 �����϶�
        else if (isdashed)
        {
            //���콺 ȸ�� �ӵ� ���̱�
            mousespeed = turnSpeed / 10;
        }
        //�̵�
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        myTR.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        //ȸ��
        float r = Input.GetAxisRaw("Mouse X");
        myTR.Rotate(Vector3.up * mousespeed * Time.deltaTime * r);

        //���� �ν�Ʈ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //���� ��������
            if (!isJump)
            {
                isJump = true;
                boosterimpact.Play();
                rigid.velocity = new Vector3(0, JumpPower, 0);
                isboost = true;
            }
            //���� ���� �ν�Ʈ
            else if (isJump)
            {
                isboost = true;
            }
        }
        //���� �ν�Ʈ �����϶�
        if (isboost)
        {
            boost.SetActive(true);
            rigid.useGravity = false;
            myTR.position += Vector3.up * 0.06f;
        }
        //���� �ν�Ʈ ����
        if (Input.GetKeyUp(KeyCode.Space))
        {
            boost.SetActive(false);
            rigid.useGravity = true;
            isboost = false;
        }
        //�ν�Ʈ
        if (Input.GetKeyDown(KeyCode.LeftShift) && ismove && dcooltime <= 0 && AP>0)
        {
            // ���� ������ �������� �ϴ� ���͸� ����ϴ�.
            rigid.velocity = Vector3.zero;
            //�뽬 ����
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
            //�뽬 ���� �� ��
            Vector3 forceDirection = dashvector * dashpower;

            //�ڷ�ƾ�۵�
            StartCoroutine(Dashjinheng(0.22f));
            //����Ʈ �۵�
            myanim.SetTrigger("dash");
            myanim.SetInteger("dashing", 0);
            boosterimpact.Play();
            // AddForce�� ���� ������ �������� �ϴ� ���� ���մϴ�.
            rigid.AddForce(forceDirection, ForceMode.Impulse);
            //��Ÿ��
            dcooltime = dashcooltime;
            //���¹̳� �Ҹ�
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

        //���¹̳�
        if (!isboost && !isdashed)
        {
            //�ν�Ʈ,���� ���� Ȯ�ο�
            dashTime = 0;
            //���¹̳� ȸ��
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
                //���� ����
                boost.SetActive(false);
                rigid.useGravity = true;
                isboost = false;
            }
            regenAPStart = false;
            regenAPTime = 0;
            //�ð� ����
            dashTime += Time.deltaTime;
            //1�ʸ���
            if (dashTime > 1)
            {
                AP = AP - 8;
                dashTime = 0;
            }
        }
        //AP ���¹̳� ȸ��
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
    //�÷��̾� �̵� �ִϸ��̼�
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
    //�÷��̾� ���� �ν�Ʈ �ִϸ��̼�
    IEnumerator Dashjinheng(float time)
    {
        if (AP < 8)
        {
            //�뽬 �ʱ�ȭ
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
    //�浹 �̺�Ʈ
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
        }
    }

}
