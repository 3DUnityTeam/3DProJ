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

    public float �������� = 10f;
    public float �������� = 10f;
    public float ��ܰ��� = 10f;
    public float �ϴܰ��� = 10f;

    private void Awake()
    {
    }

    private void Start()
    {
        float limit = GameManager.instance.AimManager.limitAngle;
        float angle = Mathf.Acos(limit) * (180 / Mathf.PI);

        �������� = Mathf.Acos(angle);
        �������� = Mathf.Acos(angle);
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
                
                // �ڽ� ��ü�� localrotation�� ������ �� ���ʹϾ����� ����
                

                break;


        }
        // ������Ʈ�� y, z �� ȸ���� ����
        Vector3 trangle = transform.localEulerAngles;
        trangle.x = (trangle.x > 180) ? trangle.x - 360 : trangle.x;
        trangle.y = (trangle.y > 180) ? trangle.y - 360 : trangle.y;
        trangle.x = Mathf.Clamp(trangle.x, -(�ϴܰ���), ��ܰ���);
        trangle.y = Mathf.Clamp(trangle.y, -(��������), ��������);
        trangle.z = 0;
        transform.localEulerAngles = trangle;


        // ���ο� ȸ������ ����


    }

    IEnumerator CheckTarget()
    {
        yield return new WaitForSeconds(0.1f);
        if (aimManager.aimingTarget != null)
        {
            target = aimManager.aimingTarget.transform;
            state = State.Targeted;
        }
        else
        {
            target = basic;
            state = State.noTarget;
        }
    }
}
