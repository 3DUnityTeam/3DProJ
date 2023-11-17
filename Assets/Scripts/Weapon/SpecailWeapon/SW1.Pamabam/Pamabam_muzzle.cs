using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pamabam_muzzle : MonoBehaviour
{
    public Transform target;
    public string enemyTag = "Mob";
    public float range = 20f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTraget", 0f, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.LookAt(target.position);
            Vector3 trangle = transform.localEulerAngles;
            trangle.z = 0;
            // 자식 개체의 localrotation을 제한을 줄 쿼터니언으로 설정
            transform.localEulerAngles = trangle;

        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void UpdateTraget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortesDistance = Mathf.Infinity;
        GameObject neareatEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortesDistance)
            {
                shortesDistance = distanceToEnemy;
                neareatEnemy = enemy;
            }
        }

        if (neareatEnemy != null && shortesDistance <= range)
        {
            target = neareatEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
}
