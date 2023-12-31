using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tofuwatching : MonoBehaviour
{
    public enum State
    {
        noTarget, Targeted
    };

    public State state = State.noTarget;
    public Transform basic;
    public Vector3 target;
    AimManager aimManager;

    private float 좌측각도;
    private float 우측각도;
    float limit ;
    float angle ;
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
        aimManager = GameManager.instance.AimManager;
        StartCoroutine(CheckTarget());
        //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        switch (state)
        {
            case State.noTarget:
                transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case State.Targeted:
                transform.LookAt(target);
                Vector3 trangle = transform.localEulerAngles;
                trangle.x = (trangle.x > 180) ? trangle.x - 360 : trangle.x;
                trangle.y = (trangle.y > 180) ? trangle.y - 360 : trangle.y;
                trangle.x = 0;
                trangle.y = Mathf.Clamp(trangle.y, -(좌측각도), 우측각도);
                trangle.z = 0;
                // 자식 개체의 localrotation을 제한을 줄 쿼터니언으로 설정
                transform.localEulerAngles = trangle;

                break;
        }
        // 오브젝트의 y, z 축 회전을 고정



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
            Vector3 center = aimManager.aimingTarget.GetComponent<Collider>().bounds.center;
            target = center;
            state = State.Targeted;
        }
    }
}