using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject dragon;
    public GameObject[] mobs;
    public GameObject mobSpawn;
    public Dictionary<int, GameObject> spawnMob= new Dictionary<int, GameObject>();

    Transform trans_;

    [SerializeField]
    int maxMob = 50;
    int leftMob;

    private void Awake()
    {
        spawnMob.Add(dragon.GetInstanceID(), dragon);
        trans_ = GetComponent<Transform>();
        leftMob = maxMob;
    }

    IEnumerator Start()
    {
        while (leftMob > 0)
        {
            int n = Random.Range(1, 6);
            if(leftMob -n < 0)
            {
                n = leftMob;
                leftMob = 0;
            }
            else
                leftMob -= n;
            Debug.Log("Summon" + n);
            
            for (int i = 0; i < n; i++)
            {
                float tX = trans_.position.x + Random.Range(-100f, 100f);
                float tZ = trans_.position.x + Random.Range(-100f, 100f);

                GameObject obj = Instantiate(mobs[Random.Range(0, mobs.Length)]);
                spawnMob.Add(obj.GetInstanceID(), obj);
                obj.transform.position = new Vector3(tX,1, tZ);
                obj.transform.parent = mobSpawn.transform;

                yield return new WaitForSeconds(0.4f);
            }
            yield return new WaitForSeconds(Random.Range(5, 11));
        }
        if(leftMob <= 0)
        {
            dragon.GetComponent<DragonController>().NextPhase();
        }
    }
}
