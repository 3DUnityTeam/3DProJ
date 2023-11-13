using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowCam : MonoBehaviour
{
    private Transform targetTR;
    private Transform myTR;

    [Range(2.0f, 20.0f)]
    public float distance = 10.0f;
    private float maxDistance;

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
        targetTR = GameManager.instance.focus.transform;
        myTR = GetComponent<Transform>();
        targetTR.position += new Vector3(width, 0, 0);
        targetTR.rotation = Quaternion.Euler(0, rotation, 0);
    }

    private void Update()
    {
        if (maxDistance < distance)
        {
            maxDistance = distance;
        }

    }
    // Update is called once per frame
    private void LateUpdate()
    {
        if (distance > maxDistance)
        {

        }
        myTR.position = targetTR.position + (-targetTR.forward * distance) + (Vector3.up * height);
        myTR.LookAt(targetTR.position);
    }
}
