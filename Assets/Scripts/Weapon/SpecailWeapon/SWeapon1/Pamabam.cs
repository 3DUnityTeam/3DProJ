using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pamabam : MonoBehaviour
{
    public enum State
    {
        normal, special
    };
    [Header ("외부 오브젝트")]
    public State state = State.normal;
    public Transform firePos;
    public GameObject muzzle;
    public GameObject beam;
    public string enemyTag = "Mob";
    [Header ("내부 수치")]
    public Transform target;
    public float range = 20f;
    public float Cooltime = 1.2f;
    public float bulletspeed = 2000f;
    private float cooltime = 1.2f;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        cooltime = Cooltime;
        InvokeRepeating("UpdateTraget", 0f, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {

        switch(state)
        {
            case State.normal:
                //상태가 보통일 경우
                cooltime -= Time.deltaTime;

                if (target)
                {
                    transform.LookAt(target.position);
                    Vector3 trangle = transform.localEulerAngles;
                    trangle.x = 0;
                    trangle.z = 0;
                    // 자식 개체의 localrotation을 제한을 줄 쿼터니언으로 설정
                    transform.localEulerAngles = trangle;

                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                if (cooltime <= 0)
                {
                    Shoot();
                    cooltime = Cooltime;
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    SpecialShoot();
                }

                break;

            case State.special:
                if (target)
                {
                    transform.LookAt(target.position);
                    Vector3 strangle = transform.localEulerAngles;
                    strangle.x = 0;
                    strangle.z = 0;
                    // 자식 개체의 localrotation을 제한을 줄 쿼터니언으로 설정
                    transform.localEulerAngles = strangle;
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                break;
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

    void Shoot()
    {
        if(target != null)
        {
            GameObject bullet = GameManager.instance.bulletPoolManger.Get(1);
            bullet.transform.position = firePos.position;
            bullet.transform.rotation = firePos.rotation;
            Vector3 center = target.GetComponent<Collider>().bounds.center;
            direction = (center - firePos.transform.position).normalized;
            bullet.GetComponent<Rigidbody>().AddForce(direction * bulletspeed);
        }
    }

    void SpecialShoot()
    {
        StartCoroutine(Shootstop());
        state = State.special;
        beam.SetActive(true);
        /*target = GameManager.instance.AimManager.aimingTarget.transform;
        GameObject bullet = GameManager.instance.bulletPoolManger.Get(1);
        bullet.transform.position = firePos.position;
        bullet.transform.rotation = firePos.rotation;
        direction = (target.transform.position - firePos.transform.position).normalized;
        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletspeed);*/
    }

    IEnumerator Shootstop()
    {
        yield return new WaitForSeconds(3f);
        beam.SetActive(false);
        state = State.normal;
    }
}
