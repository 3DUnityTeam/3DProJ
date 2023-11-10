using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowCam : MonoBehaviour
{
    public Transform targetTR;
    private Transform myTR;

    [Range(2.0f, 20.0f)]
    public float distance = 10.0f;

    [Range(0.0f, 10.0f)]
    public float height = 2.0f;

    [Range(-10f, 10f)]
    public float width = 0f;

    [Range(-30f, 30f)]
    public float rotation = 0f;

    public float sightspeed = 0.01f;
    private float sight;

    //public float damping = 10.0f;

    //public float targetOffset = 2.0f;

    //private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        myTR = GetComponent<Transform>();
        targetTR.position += new Vector3(width, 0, 0);
        targetTR.rotation = Quaternion.Euler(0, rotation, 0);
    }

    private void Update()
    {
        /*if (Input.GetKey(KeyCode.D))
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                sight = sightspeed * 5f;
            }
            else
            {
                sight = sightspeed;
            }
            // 로컬 좌표로 이동
            
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                sight = -(sightspeed * 5f);
            }
            else
            {
                sight = -sightspeed;
            }
            // 로컬 좌표로 이동
            
        }
        else
        {
            if(targetTR.localPosition.x <-0.01f)
            {
                sight = (sightspeed * 0.7f);
            }
            else if(targetTR.localPosition.x > 0.01f)
            {
                sight = -(sightspeed * 0.7f);
            }
            else
            {
                sight = 0f;
            }
        }*/

        // 로컬 x 좌표를 -3에서 3 사이로 제한

        /*targetTR.localPosition += new Vector3(sight, 0, 0);
        targetTR.localPosition = new Vector3(Mathf.Clamp(targetTR.localPosition.x, -3f, 3f), targetTR.localPosition.y, targetTR.localPosition.z);*/
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        myTR.position = targetTR.position + (-targetTR.forward * distance) + (Vector3.up * height);
        myTR.LookAt(targetTR.position);
        /*Vector3 pos = targetTR.position + (-targetTR.forward * distance) + (Vector3.up * height);
        myTR.position = Vector3.SmoothDamp(myTR.position, pos, ref velocity, damping);
        myTR.LookAt(targetTR.position + (targetTR.up * targetOffset));*/
    }
}
