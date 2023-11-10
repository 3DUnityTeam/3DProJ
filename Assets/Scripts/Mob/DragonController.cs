using System.Collections;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    public GameObject[] mobs;
    public GameObject[] fxs;  //bounce eff, rolling eff, flame, Meteo summon eff
    public GameObject[] bodyFxs;
    public GameObject[] meteos;   //blueberry poz blueberry bomb
    Player player;

    Transform trans_;
    Transform playerTrans_;
    Rigidbody rigid_;
    Animator ani_;

    Vector3 dirr = Vector3.zero;

    public int skillDmg;
    [SerializeField]
    int maxMob = 50;
    int leftMob;
    [SerializeField]
    int maxHunger = 100;
    int nowHunger = 0;

    float waitingTime;
    float speed = 10f;

    public bool meteoAll = false;

    bool flag = false;
    bool looking = true;
    bool moveLock = true;
    bool dmgFlag = false;

    bool flame = false;
    bool rolling = false;
    bool normalAtk = false;
    bool flingAtk = false;

    private void Awake()
    {
        leftMob = maxMob;
        playerTrans_ = GameObject.FindGameObjectWithTag("Player").transform;
        trans_ = GetComponent<Transform>();
        rigid_ = GetComponent<Rigidbody>();
        ani_ = GetComponent<Animator>();
        BodyFx(false);

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (!player)
            Debug.Log("Player is missing");
    }


    IEnumerator Start()
    {
        while (leftMob > 0)
        {
            int n = Random.Range(1, 6);
            leftMob -= n;
            Debug.Log("Summon" + n);
            for(int i = 0; i < n; i++)
            {
                ani_.SetBool("Summon",true);
                Instantiate(mobs[Random.Range(0, mobs.Length)],
                    new Vector3(trans_.position.x, trans_.position.y, trans_.position.z + 5),    //spawn dragon's front
                    trans_.rotation);
                yield return new WaitForSeconds(0.4f);
                Instantiate(fxs[0], new Vector3(trans_.position.x, 0, trans_.position.z), trans_.rotation);
                ani_.SetBool("Summon", false);
            }
            yield return new WaitForSeconds(Random.Range(5,11));
        }
        ani_.SetTrigger("Phase2");
        Debug.Log("Phase2 start");
        dirr = Vector3.forward;
        StartCoroutine("Phase2");
    }

    private void Update()
    {
        if(looking)
            trans_.LookAt(playerTrans_);

        float dit = Vector3.Distance(playerTrans_.position, trans_.position);
        if(dit >= 5)
            trans_.Translate(dirr * speed * Time.deltaTime);
        else
        {
            trans_.Translate(dirr * 0 * Time.deltaTime);
            ani_.SetTrigger("Atk");
        }

        if (flame)
        {
            Vector3 dir = playerTrans_.position - this.transform.position;
            float turnSpeed = 0.4f;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * turnSpeed);
        }
    }

    IEnumerator Phase2()
    {
        while(nowHunger < maxHunger)
        {
            if (nowHunger >= maxHunger)
            {
                BeHappy();
            }
            else
            {

                int p = Random.Range(4, fxs.Length);
                switch (p)
                {
                    case 0:
                        break;
                    case 1:
                        StartCoroutine("RollingAtk");
                        break;
                    case 2:
                        StartCoroutine("FlameShot");
                        break;
                    case 3:
                        StartCoroutine("FlyAtk");
                        break;
                    case 4:
                        StartCoroutine("Meteo");
                        break;
                    default:
                        Debug.Log("Dragon Script: Out of index!");
                        break;
                }
                yield return new WaitForSeconds(waitingTime + Random.Range(2f, 5.1f));
            }
        }
    }

    //Rolling toward player, 1~5times
    IEnumerator RollingAtk()
    {
        if (!flag)
        {
            flag = true;
            BodyFx(true);
            speed = 18;
            int x = Random.Range(1, 6);
            Debug.Log("Rolling " + x+ "times!");
            waitingTime = (1.3f + x * 3f);
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
            speed = 10;
            dirr = Vector3.forward;
            BodyFx(false);
            flag = false;
        }
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
            waitingTime = 5.2f;
            looking = false;
            dirr = Vector3.zero;
            yield return new WaitForSeconds(0.2f);
            ani_.speed = 0.0f;
            fxs[2].SetActive(true);
            flame = true;

            /*
            Vector3 vec = playerTrans_.position - trans_.position;
            vec.Normalize();
            Quaternion q = Quaternion.LookRotation(vec);
            float rY = q.y;
            trans_.rotation = Quaternion.Euler(0, rY, 0);
            for(int i = 0; i <60 ; i++)
            {
                trans_.rotation = Quaternion.Euler(0, rY - i, 0);
                yield return new WaitForSeconds(0.0415f);
            }
            for (int i = 0; i < 60; i++)
            {
                trans_.rotation = Quaternion.Euler(0, rY + i, 0);
                yield return new WaitForSeconds(0.0415f);
            }
            */
            yield return new WaitForSeconds(waitingTime);
            flame = false;
            fxs[2].SetActive(false);
            ani_.SetBool("Flame", false);
            ani_.speed = 1.0f;
            looking = true;
            dirr = Vector3.forward;
            BodyFx(false);
            flag = false;
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
                waitingTime = 2.3f;
                Debug.Log("Dash!");
                ani_.SetBool("Dash", true);
                looking = false;

                trans_.position = new Vector3(trans_.position.x, trans_.position.y - 2f, trans_.position.z);

                dirr = Vector3.forward;
                speed = 20f;
                yield return new WaitForSeconds(2.3f);

                ani_.SetBool("Dash", false) ;
                ani_.SetBool("Fly", false);
                rigid_.useGravity = true;
                speed = 10f;
                looking = true;
            }
            else //Do it later
            {
                waitingTime = 5.2f;
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
            BodyFx(false);
            flag = false;
        }
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
            Debug.Log("After Spining");
            fxs[4].SetActive(true);
            Debug.Log("Fx On");
            //if(Random.Range(0, 1) == 0)  //한번에 랜덤한 위치로 10~20개의 메테오 분출
            //{
                Debug.Log("Before selecting N");
                int n = Random.Range(10, 21);
                Debug.Log("N="+n);
                waitingTime = (float)n * 0.1f + 1.5f;
                Debug.Log("Summon " + n + "meteors!!");
                Vector3 nowPoz = trans_.position;
                for(int i = 0; i < n; i++)
                {
                    float x = Random.Range(-50f, 50f);
                    float z = Random.Range(-50f, 50f);
                    Vector3 spawnPoz = nowPoz + new Vector3(x, 0, z);
                    GameObject land = GameObject.FindWithTag("Land");
                    GameObject obj =  Instantiate(meteos[0]);
                    obj.transform.position = spawnPoz;
                    obj.name = "MeteoPoz " + n;

                    spawnPoz += new Vector3(0, 10, 0);
                    obj = Instantiate(meteos[1]);
                    obj.transform.position = spawnPoz;
                    obj.name = "Meteo " + n;
                    yield return new WaitForSeconds(0.3f);
                }
                yield return new WaitForSeconds(1.5f);
                meteoAll = true;
            //}

            yield return new WaitForSeconds(0.1f);
            fxs[4].SetActive(false);
            ani_.SetBool("Spin", false);
            looking = true;
            dirr = Vector3.forward;
            BodyFx(false);
            meteoAll = false;
            flag = false;
        }

    }

    //full happy dragon
    void BeHappy()
    {
        dirr = Vector3.zero;
        ani_.SetTrigger("Happy");
        nowHunger = maxHunger;
    }

    void BodyFx(bool t)
    {
        bodyFxs[0].SetActive(t);
        bodyFxs[1].SetActive(t);
        bodyFxs[2].SetActive(t);
        bodyFxs[3].SetActive(t);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        { 
            nowHunger++;
        }
    }

    private void OnTriggerEnter(Collider other)  //드래곤 몸통 데미지
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (rolling)
            {
                StartCoroutine(DMGtoPlayer(8, 0.5f));
            }

            if (normalAtk)
            {
                StartCoroutine(DMGtoPlayer(5, 1.5f));
            }

            if (flingAtk)
            {
                StartCoroutine(DMGtoPlayer(8, 0.5f));
            }
        }
    }

    IEnumerator DMGtoPlayer(float dmg, float time)
    {
        if (!dmgFlag)
        {
            dmgFlag = true;
            player.HP -= dmg;
            yield return new WaitForSeconds(time);
            dmgFlag = false;
        }
    }
}
