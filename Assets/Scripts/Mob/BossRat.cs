using System.Collections;
using UnityEngine;

public class BossRat : MobParent
{
    public GameObject pm;
    public GameObject heart;
    public GameObject Fxs; //Summon
    public GameObject Player;
    public GameObject mobSpawn;
    public GameObject[] meteos;
    Transform trans_;
    Animator ani_;

    public float traceDist = 6.5f;
    public int summons = 50;
    public int mobs;

    bool isWatching = false;
    bool flag = false;
    bool isFound = false;

    bool deadCheck = false;
    bool mobClear = false;

    private void Awake()
    {
        personalColor = Color.yellow;
        MaxHP = 2500f;
        mobs = summons;
        trans_ = GetComponent<Transform>();
        ani_ = GetComponent<Animator>();
    }


    private void Update()
    {
        if (!Dead)
        {
            if (mobs <= 0)
                mobClear = true;
            else
                HP = 0;

            StartCoroutine(CheckState());

            if (HP >= MaxHP)
            {
                BeHappy();
            }

            if (isWatching)
            {

                Vector3 dir = Player.transform.position - this.transform.position;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2);

                //trans_.LookAt(new Vector3(Player.transform.position.x, trans_.position.y, Player.transform.position.z));
            }
        }
        else
            return;
    }


    IEnumerator Summon()
    {
        if (!flag)
        {
            flag = true;
            Fxs.SetActive(true);
            ani_.SetBool("Summon", true);
            for (int i = 0; i < summons; i++)
            {
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
                    obj.name = "Lemon";

                    summons--;
                    yield return new WaitForSeconds(0.95f);
                }
            }
            //summons = 0;
            Fxs.SetActive(false);
            ani_.SetBool("Summon", false);
            flag = false;
        }
    }

    IEnumerator Meteos()
    {
        if (!flag)
        {
            flag = true;
            Fxs.SetActive(true);
            ani_.SetBool("Meteo", true);
            int n = Random.Range(10, 21);
            for (int i = 0; i < n; i++)
            {
                if (Dead)
                    break;
                float x = Random.Range(-50f, 50f);
                float z = Random.Range(-50f, 50f);
                Vector3 spawnPoz = trans_.position + new Vector3(x, -2, z);
                GameObject obj = Instantiate(meteos[0]);
                obj.transform.position = spawnPoz;
                obj.name = "MeteoPoz " + i;

                spawnPoz += new Vector3(0, 30, 0);
                obj = Instantiate(meteos[1]);
                obj.transform.position = spawnPoz;
                obj.name = "Meteo " + i;
                yield return new WaitForSeconds(0.1f);
            }
            Fxs.SetActive(false);
            ani_.SetBool("Meteo", false);
            yield return new WaitForSeconds(5.5f);
            flag = false;
        }
    }
    

    IEnumerator CheckState()
    {
        float dist = Vector3.Distance(Player.transform.position, trans_.position);

        if (dist <= traceDist)
        {
            isFound = true;
            if (summons > 0)
            {
                StartCoroutine(Summon());
            }
            else
            {
                if (!Dead)
                {
                    isWatching = true;
                    StartCoroutine(Meteos());
                }
            }
        }
        else
        {
            isFound = false;
            isWatching = false;
            ani_.SetBool("Summon", false);
            Fxs.SetActive(false);
            ani_.SetBool("Meteo", false);
        }

        yield return new WaitForSeconds(0.3f);
    }

    void BeHappy()
    {
        ani_.SetTrigger("Happy");
        HP = MaxHP;
        if (!deadCheck)
        {
            GameManager.instance.progressManager.boss2Cleared = true;
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
        GameManager.instance.progressManager.Clear(2);
    }
}