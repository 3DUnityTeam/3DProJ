using System.Collections;
using UnityEngine;

public class BossSnake : MonoBehaviour
{
    public GameObject[] Fxs; //Summon, Fire Breath
    public GameObject Player;
    Transform trans_;
    Animator ani_;

    public float traceDist = 6.5f;
    int summons = 50;

    bool isWatching = false;
    bool timeFlag = false;
    bool atkFlag = false;

    bool isFlame = false;

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

    private void FixedUpdate()
    {

    }

    IEnumerator Timmer(float time)
    {
        if (!timeFlag && atkFlag)
        {
            timeFlag = true;
            yield return new WaitForSeconds(time);
            isFlame = false;
            atkFlag = false;
            timeFlag = false;
        }
    }

    IEnumerator CheckState()
    {
        float dist = Vector3.Distance(Player.transform.position, trans_.position);

        if (dist <= traceDist)
        {
            Debug.Log("Snake found player");
            isWatching = true;
        }
        else
        {
        }

        yield return new WaitForSeconds(0.3f);
    }

    void IsDead()
    {
        ani_.SetTrigger("Happy");
        Destroy(this.gameObject, 2.5f);
    }
}
