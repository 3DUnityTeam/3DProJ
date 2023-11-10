using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunmove : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        // ������Ʈ�� y, z �� ȸ���� ����
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // ������Ʈ�� x ���� target�� ��ġ�� ���ϵ��� ȸ��
        transform.LookAt(target.position, Vector3.up);
        float xRotation = transform.rotation.eulerAngles.x; // ���� x �� ȸ�� ���� ����

        // x �� ȸ�� ���� �����Ͽ� ������Ʈ�� Ư�� ������ ȸ������ �ʵ��� ��
        /*if (xRotation > 90f)
        {
            xRotation = 90f;
        }
        else if (xRotation < 0f)
        {
            xRotation = 0f;
        }*/

        // ���ο� ȸ������ ����
        transform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
