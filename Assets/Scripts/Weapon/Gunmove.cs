using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunmove : MonoBehaviour
{
    public enum State
    {
        noTarget,Targeted
    };

    public State state = State.noTarget;
    public Transform basic;
    public Transform target;
    AimManager aimManager;

    public float 좌측각도 = 10f;
    public float 우측각도 = 10f;
    public float 상단각도 = 10f;
    public float 하단각도 = 10f;

    private void Awake()
    {
    }

    private void Start()
    {
        float limit = GameManager.instance.AimManager.limitAngle;
        float angle = Mathf.Acos(limit) * (180 / Mathf.PI);

        좌측각도 = Mathf.Acos(angle);
        우측각도 = Mathf.Acos(angle);
    }
    void Update()
    {
        aimManager=GameManager.instance.AimManager;
        StartCoroutine(CheckTarget());
        //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        switch (state)
        {
            case State.noTarget:
                transform.LookAt(basic.position);
                //float xRotation = transform.rotation.eulerAngles.x;
                //transform.localEulerAngles = transform.localEulerAngles;
                //transform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                break;
            case State.Targeted:
                Vector3 center = target.GetComponent<Collider>().bounds.center;
                transform.LookAt(center);
                
                // 자식 개체의 localrotation을 제한을 줄 쿼터니언으로 설정
                

                break;


        }
        // 오브젝트의 y, z 축 회전을 고정
        Vector3 trangle = transform.localEulerAngles;
        trangle.x = (trangle.x > 180) ? trangle.x - 360 : trangle.x;
        trangle.y = (trangle.y > 180) ? trangle.y - 360 : trangle.y;
        trangle.x = Mathf.Clamp(trangle.x, -(하단각도), 상단각도);
        trangle.y = Mathf.Clamp(trangle.y, -(좌측각도), 우측각도);
        trangle.z = 0;
        transform.localEulerAngles = trangle;


        // 새로운 회전값을 적용


    }

    IEnumerator CheckTarget()
    {
        yield return new WaitForSeconds(0.1f);
        if (aimManager.aimingTarget == null)
        {
            state = State.noTarget;
        }
        else if (aimManager.aimingTarget != null)
        {
            target = aimManager.aimingTarget.transform;
            state = State.Targeted;
        }
    }
}
