using System.Collections;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    public GameObject[] mobs;
    public GameObject[] fxs;  //bounce eff, rolling eff, flame, 

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
    [SerializeField]
    float speed = 30f;

    bool flag = false;
    bool looking = true;
    bool moveLock = true;

    private void Awake()
    {
        leftMob = maxMob;
        playerTrans_ = GameObject.FindGameObjectWithTag("Player").transform;
        trans_ = GetComponent<Transform>();
        rigid_ = GetComponent<Rigidbody>();
        ani_ = GetComponent<Animator>();
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
        
        trans_.Translate(dirr * speed * Time.deltaTime);
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
                int p = Random.Range(0, fxs.Length);
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
                    default:
                        break;
                }
                yield return new WaitForSeconds(waitingTime);
            }
        }
    }

    //Rolling toward player, 1~5times
    IEnumerator RollingAtk()
    {
        if (!flag)
        {
            flag = true;
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
            speed = 15;
            dirr = Vector3.forward;
            flag = false;
        }
    }

    //flame shot
    IEnumerator FlameShot()
    {
        if (!flag)
        {
            flag = true;
            Debug.Log("Flame Shot!");
            ani_.SetBool("Flame", true);
            waitingTime = 5.2f;
            looking = false;
            dirr = Vector3.zero;
            yield return new WaitForSeconds(0.2f);
            ani_.speed = 0.0f;
            float rY = trans_.rotation.y + 30;
            fxs[2].SetActive(true);
            trans_.rotation = Quaternion.Euler(0, rY, 0);
            for(int i = 0; i <60 ; i++)
            {
                trans_.rotation = Quaternion.Euler(0, rY - 1, 0);
                yield return new WaitForSeconds(0.0415f);
            }
            for (int i = 0; i < 60; i++)
            {
                trans_.rotation = Quaternion.Euler(0, rY + 1, 0);
                yield return new WaitForSeconds(0.0415f);
            }
            fxs[2].SetActive(false);
            ani_.SetBool("Flame", false);
            ani_.speed = 1.0f;
            looking = true;
            dirr = Vector3.forward;
            flag = false;
        }
    }

    //fly + dash or flame shot
    IEnumerator FlyAtk()
    {
        if (!flag)
        {
            flag = true;
            Debug.Log("Fly!");
            ani_.SetBool("Fly", true);
            fxs[3].SetActive(true);
            rigid_.useGravity = false;
            trans_.position = new Vector3(trans_.position.x, trans_.position.y + 3.5f, trans_.position.z);

            int r = Random.Range(0, 2);
            if(r == 0)
            {
                waitingTime = 2.3f;
                Debug.Log("Dash!");
                ani_.SetBool("Dash", true);
                looking = false;
                dirr = Vector3.forward;
                rigid_.useGravity = true;
                speed = 20f;
                yield return new WaitForSeconds(2.3f);
                ani_.SetBool("Dash", false) ;
                speed = 15f;
                looking = true;
            }
            else
            {
                waitingTime = 5.2f;
                Debug.Log("Flame Bomb!");
                trans_.position = new Vector3(trans_.position.x, trans_.position.y + 3.5f, trans_.position.z);
                ani_.SetBool("Flame", true);
                yield return new WaitForSeconds(0.2f);
                ani_.speed = 0;
                dirr = Vector3.zero;
                rigid_.useGravity = true;
                ani_.SetBool("Flame", false);
                ani_.speed = 1;
                yield return new WaitForSeconds(5f);
            }
            fxs[3].SetActive(false);
            ani_.SetBool("Fly", false);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        { 
            nowHunger++;
        }
    }
}
