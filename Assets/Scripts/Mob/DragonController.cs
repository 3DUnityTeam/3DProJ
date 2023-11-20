using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragonController : MobParent
{
    public GameObject heart;
    //GameManager`s manager
    GameObject mobSpawn;
    Player player;
    Transform playerTrans_;

    //UnityCompent
    Transform trans_;
    Rigidbody rigid_;
    Animator ani_;

    bool deadCheck = false;


    [Header("#Pattern")]
    public GameObject[] fxs;  //bounce eff, rolling eff, flame, Meteo summon eff
    public GameObject[] bodyFxs;
    public GameObject[] meteos;   //blueberry poz blueberry bomb

    [Header("#DirectDamage")]
    public float rollingDamage=16;
    public float normalAtkDamage =10;
    public float flingAtkDamage =16;

    Vector3 dirr = Vector3.zero;
    Vector3 resetPoz = new Vector3(-95, 8f, 19);

    public int skillDmg;
    [Header("#SpawnMobCount")]
    [SerializeField]
    int maxMob = 50;
    int leftMob;

    //float waitingTime;
    float speed = 18f;

    bool flag = false;
    bool looking = true;
    bool atkFlag = false;

    bool flame = false;
    bool rolling = false;
    bool normalAtk = false;
    bool flingAtk = false;
    bool isEnd = false;

    private void Awake()
    {
        MaxHP = 4000f;
        HP = 0;

        leftMob = maxMob;
        trans_ = GetComponent<Transform>();
        rigid_ = GetComponent<Rigidbody>();
        ani_ = GetComponent<Animator>();
        BodyFx(false);
    }
    private new void Start()
    {
        base.Start();
        personalColor = new Color(1, 0, 1);
        player = GameManager.instance.player;
        mobSpawn = GameManager.instance.SpawnManager.gameObject;

        playerTrans_ = player.transform;

         NextPhase();
    }

    private void FixedUpdate()
    {
        if (Dead)
            return;
        if (HP >= MaxHP)
        {
            BeHappy();
        }

        if (looking)
            trans_.LookAt(new Vector3(playerTrans_.position.x, trans_.position.y, playerTrans_.position.z));

        float dit = Vector3.Distance(playerTrans_.position, trans_.position);
        if (dit >= 10)
        {
            trans_.Translate(dirr * speed * Time.fixedDeltaTime);
            normalAtk = false;
        }
        else
        {
            trans_.Translate(dirr * 0 * Time.fixedDeltaTime);
            if (!atkFlag)
                ani_.SetTrigger("Atk");
            normalAtk = true;
        }

        if (flame)
        {
            Vector3 dir = playerTrans_.position - this.transform.position;
            float turnSpeed = 0.4f;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.fixedDeltaTime * turnSpeed);
        }
    }

    public void NextPhase()
    {
        ani_.SetTrigger("Phase2");
        Debug.Log("Phase2 start");
        BodyFx(false);
        fxs[4].SetActive(false);
        dirr = Vector3.forward;
        StartCoroutine("Phase2");
    }

    IEnumerator Phase2()
    {
        looking = true;
        yield return new WaitForSeconds(4.5f);
        isEnd = true;
    }

    private void Update()
    {
        if (HP < MaxHP)
        {
            if (trans_.position.x < -210 || trans_.position.x > 210 
                || trans_.position.z < -210 || trans_.position.z > 210)
            {
                trans_.position = resetPoz;
            }


            if (isEnd)
            {
                isEnd = false;

                looking = true;
                atkFlag = true;
                ani_.SetTrigger("Reset");
                int p = Random.Range(0, fxs.Length + 1);
                //int p = 4;
                Debug.Log("Pattern: " + p);
                switch (p)
                {
                    case 0:
                        StartCoroutine(JustWalk());
                        break;
                    case 1:
                        StartCoroutine(RollingAtk());
                        break;
                    case 2:
                        StartCoroutine(FlameShot());
                        break;
                    case 3:
                        StartCoroutine(FlyAtk());
                        break;
                    case 4:
                        StartCoroutine(Meteo());
                        break;
                    case 5:
                        StartCoroutine(Summon());
                        break;
                    default:
                        Debug.Log("Dragon Script: Out of index!");
                        break;
                }
                atkFlag = false;
            }
        }
    }

    IEnumerator JustWalk()
    {
        yield return new WaitForSeconds(3.5f);
        isEnd = true;
    }

    //Rolling toward player, 1~5times
    IEnumerator RollingAtk()
    {
        if (!flag)
        {
            flag = true;
            BodyFx(true);
            speed = 30;  //Rolling speed
            int x = Random.Range(1, 6);
            Debug.Log("Rolling " + x+ "times!");
            //waitingTime = (1.3f + x * 3f);
            ani_.SetBool("Rolling", true);
            yield return new WaitForSeconds(1.3f);
            for (int i = 0; i < x; i++)
            {
                dirr = Vector3.forward;
                looking = false;
                for(int j = 0; j < 27; j++)
                {
                    Instantiate(fxs[1], new Vector3(trans_.position.x, -0.2f, trans_.position.z), trans_.rotation);
                    yield return new WaitForSeconds(0.1f);
                }
                //yield return new WaitForSeconds(2.7f);
                dirr = Vector3.zero;
                yield return new WaitForSeconds(0.2f);
                looking = true;
                yield return new WaitForSeconds(0.1f);
            }
            ani_.SetBool("Rolling", false);
            speed = 18;
            dirr = Vector3.forward;
            BodyFx(false);
            flag = false;
            isEnd = true;
        }
        Debug.Log("Rolling Out");
    }

    //flame shot
    IEnumerator FlameShot()
    {
        if (!flag)
        {
            flag = true;
            BodyFx(true);
            Debug.Log("Flame Shot!");
            ani_.SetBool("Flame", true);
            //waitingTime = 5.2f;
            looking = false;
            dirr = Vector3.zero;
            yield return new WaitForSeconds(0.2f);
            ani_.speed = 0.0f;
            fxs[2].SetActive(true);
            flame = true;
            yield return new WaitForSeconds(5.2f);
            flame = false;
            fxs[2].SetActive(false);
            ani_.SetBool("Flame", false);
            ani_.speed = 1.0f;
            looking = true;
            dirr = Vector3.forward;
            BodyFx(false);
            flag = false;
            isEnd = true;
        }
    }

    //fly + dash or flame shot
    IEnumerator FlyAtk()
    {
        if (!flag)
        {
            flag = true;
            BodyFx(true);
            Debug.Log("Fly!");
            ani_.SetBool("Fly", true);
            fxs[3].SetActive(true);
            rigid_.useGravity = false;
            trans_.position = new Vector3(trans_.position.x, trans_.position.y + 3.5f, trans_.position.z);

            int r = Random.Range(0, 1);
            if(r == 0)
            {
                //waitingTime = 2.3f;
                Debug.Log("Dash!");
                ani_.SetBool("Dash", true);
                looking = false;

                trans_.position = new Vector3(trans_.position.x, trans_.position.y - 2f, trans_.position.z);

                dirr = Vector3.forward;
                speed = 32f;
                yield return new WaitForSeconds(2.3f);

                ani_.SetBool("Dash", false) ;
                ani_.SetBool("Fly", false);
                rigid_.useGravity = true;
                speed = 18f;
            }
            else //Do it later
            {
                //waitingTime = 5.2f;
                Debug.Log("Flame Bomb!");
                trans_.position = new Vector3(trans_.position.x, trans_.position.y + 3.5f, trans_.position.z);
                
                ani_.SetBool("Flame", true);
                yield return new WaitForSeconds(0.2f);
                ani_.speed = 0;
                dirr = Vector3.zero;


                yield return new WaitForSeconds(5f);
                rigid_.useGravity = true;
                ani_.SetBool("Flame", false);
                ani_.speed = 1;
            }
            fxs[3].SetActive(false);
            looking = true;
            BodyFx(false);
            flag = false;
            isEnd = true;
        }
        Debug.Log("Fly out");
    }

    IEnumerator Meteo()
    {
        if (!flag)
        {
            flag = true;
            looking = false;
            dirr = Vector3.zero;
            BodyFx(true);
            ani_.SetBool("Spin", true);
            //Debug.Log("After Spining");
            fxs[4].SetActive(true);
            //Debug.Log("Fx On");
            if(Random.Range(0, 2) == 0)  //�ѹ��� ������ ��ġ�� 10~30���� ���׿� ����
            {
                //Debug.Log("Before selecting N");
                int n = Random.Range(40, 61);
                //Debug.Log("N="+n);
                //waitingTime = (float)n * 0.1f + 1.5f;
                Debug.Log("Summon " + n + "meteors!!");
                Vector3 nowPoz = trans_.position;
                for(int i = 0; i < n; i++)
                {
                    float x = Random.Range(-150f, 150f);
                    float z = Random.Range(-150f, 150f);
                    Vector3 spawnPoz = nowPoz + new Vector3(x, -2, z);
                    GameObject obj =  Instantiate(meteos[0]);
                    obj.transform.position = spawnPoz;
                    obj.name = "MeteoPoz " + i;

                    spawnPoz += new Vector3(0, 30, 0);
                    obj = Instantiate(meteos[1]);
                    obj.transform.position = spawnPoz;
                    obj.name = "Meteo " + i;
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(1.5f);
            }
            else  //���׿� 5��~10���� �÷��̾� �Ӹ����� ����
            {
                int n = Random.Range(10, 21);
                //waitingTime = 0.65f * n;
                Debug.Log(n + "Meteos!");
                for(int i = 0; i < n; i++)
                {
                    float x = playerTrans_.position.x;
                    float z = playerTrans_.position.z;
                    Vector3 spawnPoz =  new Vector3(x, -2, z);
                    GameObject obj = Instantiate(meteos[0]);
                    obj.transform.position = spawnPoz;
                    obj.name = "MeteoPoz " + i;

                    yield return new WaitForSeconds(0.65f);

                    spawnPoz += new Vector3(0, 30, 0);
                    obj = Instantiate(meteos[1]);
                    obj.transform.position = spawnPoz;
                    obj.name = "Meteo " + i;
                }

            }
            yield return new WaitForSeconds(0.1f);
            fxs[4].SetActive(false);
            ani_.SetBool("Spin", false);
            looking = true;
            dirr = Vector3.forward;
            BodyFx(false);
            flag = false;
            isEnd = true;
        }
        Debug.Log("Meteo Out");
    }

    IEnumerator Summon()
    {
        if (!flag)
        {
            flag = true;
            looking = false;
            BodyFx(true);
            dirr = Vector3.zero;
            leftMob = 10;
            Debug.Log("Total Mob: " + leftMob);

            //waitingTime = 5f;

            fxs[4].SetActive(true);
            ani_.SetBool("Spin", true);
            for (int i = 0; i < leftMob; i++)
                {
                    float tX = trans_.position.x + Random.Range(-100f, 100f);
                    float tZ = trans_.position.x + Random.Range(-100f, 100f);
                    int random = Random.Range(0, GameManager.instance.SpawnManager.pools.Length);
                    GameObject obj = GameManager.instance.SpawnManager.Get(random);
                    obj.transform.position = new Vector3(tX, 1, tZ);
                    obj.transform.parent = mobSpawn.transform;

                    yield return new WaitForSeconds(0.5f);
                }

             fxs[4].SetActive(false);
             ani_.SetBool("Spin", false);
           
            leftMob = 0;
            BodyFx(false);
            looking = true;
            dirr = Vector3.forward;
            flag = false;
            isEnd = true;
        }
    }

    //full happy dragon
    void BeHappy()
    {
        dirr = Vector3.zero;
        ani_.SetTrigger("Happy");
        HP = MaxHP;
        if (!deadCheck)
        {
            deadCheck = true;
            //SceneManager.LoadScene("Win");
            StartCoroutine(WaitDeadStatus());
        }
    }
    IEnumerator WaitDeadStatus()
    {
        heart.SetActive(true);
        yield return new WaitForSeconds(3);
        Dead = true;
        GameManager.instance.progressManager.Clear(3);
    }


    void BodyFx(bool t)
    {
        bodyFxs[0].SetActive(t);
        bodyFxs[1].SetActive(t);
        bodyFxs[2].SetActive(t);
        bodyFxs[3].SetActive(t);
    }

    public new void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (DirectTimer >= 1)
            {
                if (rolling)
                {
                    GameManager.instance.player.GetHitDamage(rollingDamage);
                }
                else if (normalAtk)
                {
                    GameManager.instance.player.GetHitDamage(normalAtkDamage);
                }
                else if (flingAtk)
                {
                    GameManager.instance.player.GetHitDamage(flingAtkDamage);
                }
                DirectTimer = 0;
            }
            DirectTimer += Time.fixedDeltaTime;
        }
    }
    private new void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
    }
}
