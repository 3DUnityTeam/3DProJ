using System.Collections;
using UnityEngine;

public class BossSnake : MobParent
{
    public GameObject arua;
    public GameObject heart;
    public GameObject[] Fxs; //Summon, Fire Breath
    GameObject Player;
    GameObject mobSpawn;
    Transform trans_;
    Animator ani_;

    public float traceDist = 6.5f;
    public int summons = 50;
    public int mobs;

    bool mobcleared = false;
    bool isWatching = false;
    bool flag = false;
    bool isFound = false;
    bool flag_ = false;

    bool deadCheck = false;

    private void Awake()
    {
        personalColor = Color.red;
        MaxHP = 2500f;
        mobs = summons;
        trans_ = GetComponent<Transform>();
        ani_ = GetComponent<Animator>();
    }

    private new void Start()
    {
        base.Start();
        Player = GameManager.instance.player.gameObject;
        mobSpawn = GameManager.instance.SpawnManager.gameObject;
    }

    private void Update()
    {
        if (!Dead)
        {
            if (mobs <= 0)
            {
                arua.SetActive(false);
                mobClear = true;

                if (!mobcleared)
                {
                    mobcleared = true;
                    GameManager.instance.statemessage.MassageState("뱀의 보호막이 해제됐어요!");
                }
            }
            else
                HP = 0;
            

            StartCoroutine(CheckState());
            if (HP >= MaxHP)
            {
                isWatching = false;
                BeHappy();
            }

            if (isWatching)
            {
                Vector3 dir = Player.transform.position - this.transform.position;
                trans_.rotation = Quaternion.Lerp(this.transform.rotation, 
                    Quaternion.LookRotation(dir), Time.deltaTime * 0.98f);
            }
        }
        else
        {
            Fxs[1].SetActive(false);
            return;
        }
    }

    IEnumerator FlameShot()
    {
        if (!flag)
        {
            flag = true;
            Fxs[1].SetActive(true);
            ani_.SetBool("Flame", true);
            GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.FireSpit);
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
                if (!isFound)
                    break;
                else
                {
                    float tX = trans_.position.x + Random.Range(-30f, 30f);
                    float tZ = trans_.position.x + Random.Range(-30f, 30f);
                    int random = Random.Range(0, GameManager.instance.SpawnManager.pools.Length);
                    GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.MobSpawn);
                    GameObject obj = GameManager.instance.SpawnManager.Get(random);

                    obj.transform.position = new Vector3(tX, trans_.position.y, tZ);
                    obj.transform.parent = mobSpawn.transform;
                    obj.name = "Tomato";

                    summons--;
                    yield return new WaitForSeconds(0.9f);
                }
            }
            //summons = 0;
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
                if (HP < MaxHP && mobClear)
                {
                    isWatching = true;
                    StartCoroutine(FlameShot());
                }
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

    void BeHappy()
    {
        ani_.SetTrigger("Happy");
        HP = MaxHP;
        if (!deadCheck)
        {
            if (GameManager.instance.AimManager.mobList.Contains(gameObject))
                GameManager.instance.AimManager.mobList.Remove(gameObject);
            GameManager.instance.progressManager.Clear(1);
            deadCheck = true;
            StartCoroutine(WaitDeadStatus());
        }
    }
    IEnumerator WaitDeadStatus()
    {
        heart.SetActive(true);
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Happy);
        yield return new WaitForSeconds(3);
        Dead = true;
    }
}
