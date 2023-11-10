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
    }
    void Update()
    {
        aimManager = GameManager.instance.AimManager;
        StartCoroutine(CheckTarget());
        //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        switch (state)
        {
            case State.noTarget:
                
                break;
            case State.Targeted:
                transform.LookAt(target.position);
                Vector3 trangle = transform.localEulerAngles;
                trangle.x = (trangle.x > 180) ? trangle.x - 360 : trangle.x;
                trangle.y = (trangle.y > 180) ? trangle.y - 360 : trangle.y;
                trangle.x = 0;
                trangle.y = Mathf.Clamp(trangle.y, -(��������), ��������);
                trangle.z = 0;
                // �ڽ� ��ü�� localrotation�� ������ �� ���ʹϾ����� ����
                transform.localEulerAngles = trangle;

                break;
        }
        // ������Ʈ�� y, z �� ȸ���� ����



        // ���ο� ȸ������ ����


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