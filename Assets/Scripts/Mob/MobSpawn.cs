using System.Collections;
using UnityEngine;

public class MobSpawn : MonoBehaviour
{
    public GameObject dragon;
    public GameObject[] mobs;
    public GameObject mobSpawn;

    Transform trans_;

    [SerializeField]
    int maxMob = 50;
    int leftMob;

    private void Awake()
    {
        trans_ = GetComponent<Transform>();
        leftMob = maxMob;
    }

    IEnumerator Start()
    {
        while (leftMob > 0)
        {
            int n = Random.Range(1, 6);
            leftMob -= n;
            Debug.Log("Summon" + n);
            for (int i = 0; i < n; i++)
            {
                float tX = trans_.position.x + Random.Range(-100f, 100f);
                float tZ = trans_.position.x + Random.Range(-100f, 100f);

                GameObject obj = Instantiate(mobs[Random.Range(0, mobs.Length)]);
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
