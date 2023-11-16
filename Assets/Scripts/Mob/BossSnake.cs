using System.Collections;
using UnityEngine;

public class BossSnake : MonoBehaviour
{
    public GameObject[] Fxs; //Summon, Fire Breath
    public GameObject Player;
    Transform trans_;

    bool isWatching = true;
    bool timeFlag = false;
    bool atkFlag = false;

    bool isFlame = false;
    bool isSummon = false;

    private void Awake()
    {
        trans_ = GetComponent<Transform>();
    }

    private void Update()
    {
        if (isWatching)
        {
            trans_.LookAt(Player.transform);
        }
        else
        {
            if (isFlame)
            {

            }
            if (isSummon)
            {

            }
        }
    }

    private void FixedUpdate()
    {
        if (!atkFlag)
        {
            atkFlag = true;
            int n = Random.Range(0, 3);
            switch (n)
            {
                case 0:
                    Debug.Log("Stay");
                    StartCoroutine(Timmer(3.0f));
                    break;
                case 1:    //flame
                    Debug.Log("Snake's Flame shot");
                    isWatching = false;
                    StartCoroutine(Timmer(5.0f));
                    break;
                case 2:    //summon
                    Debug.Log("Snake's summon");
                    isWatching = false;
                    StartCoroutine(Timmer(8.0f));
                    break;
            }
            isWatching = true;
        }
    }

    IEnumerator Timmer(float time)
    {
        if (!timeFlag && atkFlag)
        {
            timeFlag = true;
            yield return new WaitForSeconds(time);
            isSummon = false;
            isFlame = false;
            atkFlag = false;
            timeFlag = false;
        }
    }
}
