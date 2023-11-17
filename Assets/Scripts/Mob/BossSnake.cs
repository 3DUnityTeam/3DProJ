using System.Collections;
using UnityEngine;

public class BossSnake : MobParent
{
    public GameObject[] Fxs; //Summon, Fire Breath
    public GameObject Player;
    public GameObject mobSpawn;
    Transform trans_;
    Animator ani_;

    public float traceDist = 6.5f;
    public int summons = 50;

    bool isWatching = false;
    bool flag = false;
    bool isFound = false;

    private void Awake()
    {
        trans_ = GetComponent<Transform>();
        ani_ = GetComponent<Animator>();
    }


    private void Update()
    {
        StartCoroutine(CheckState());

        if (isWatching)
        {
            trans_.LookAt(new Vector3(Player.transform.position.x, trans_.position.y, Player.transform.position.z));
        }
    }


    IEnumerator FlameShot()
    {
        if (!flag)
        {
            flag = true;
            Fxs[1].SetActive(true);
            ani_.SetBool("Flame", true);
            yield return new WaitForSeconds(2.2f);
            Fxs[1].SetActive(false);
            ani_.SetBool("Flame", false);
            yield return new WaitForSeconds(5.2f);
            flag = false;
        }
    }

    IEnumerator Summon()
    {
        if (!flag)
        {
            flag = true;
            Fxs[0].SetActive(true);
            ani_.SetBool("Summon", true);
            for (int i = 0; i < summons; i++)
            {
                Debug.Log("4");
                if (!isFound)
                    break;
                else
                {
                    float tX = trans_.position.x + Random.Range(-10f, 10f);
                    float tZ = trans_.position.x + Random.Range(-10f, 10f);
                    int random = Random.Range(0, GameManager.instance.SpawnManager.pools.Length);
                    GameObject obj = GameManager.instance.SpawnManager.Get(random);
                    obj.transform.position = new Vector3(tX, trans_.position.y, tZ);
                    obj.transform.parent = mobSpawn.transform;

                    summons--;
                    yield return new WaitForSeconds(0.95f);
                }
            }
            summons = 0;
            Fxs[0].SetActive(false);
            ani_.SetBool("Summon", false);
            flag = false;
        }
    }

    IEnumerator CheckState()
    {
        float dist = Vector3.Distance(Player.transform.position, trans_.position);

        if (dist <= traceDist)
        {
            isFound = true;
            if(summons > 0)
            {
                StartCoroutine(Summon());
            }
            else
            {
                isWatching = true;
                StartCoroutine(FlameShot());
            }
        }
        else
        {
            isFound = false;
            isWatching = false;
            ani_.SetBool("Summon", false);
            Fxs[0].SetActive(false);
            ani_.SetBool("Flame", false);
            Fxs[1].SetActive(false);
        }

        yield return new WaitForSeconds(0.3f);
    }

    void IsDead()
    {
        ani_.SetTrigger("Happy");
        Destroy(this.gameObject, 2.5f);
    }
}
