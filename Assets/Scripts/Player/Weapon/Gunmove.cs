using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunmove : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        // 오브젝트의 y, z 축 회전을 고정
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // 오브젝트의 x 축을 target의 위치를 향하도록 회전
        transform.LookAt(target.position, Vector3.up);
        float xRotation = transform.rotation.eulerAngles.x; // 현재 x 축 회전 값을 얻음

        // x 축 회전 값을 조절하여 오브젝트가 특정 각도로 회전하지 않도록 함
        /*if (xRotation > 90f)
        {
            xRotation = 90f;
        }
        else if (xRotation < 0f)
        {
            xRotation = 0f;
        }*/

        // 새로운 회전값을 적용
        transform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
