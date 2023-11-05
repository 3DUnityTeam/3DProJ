using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ces_MovePly : MonoBehaviour
{
    public float v;
    public float h;
    float speed = 4f;
    Rigidbody rig;
    Transform trans;

    private float xRotate, yRotate, xRotateMove, yRotateMove;
    public float rotateSpeed = 500.0f;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        trans.Translate((h*Vector3.back+v*Vector3.right) * speed * Time.deltaTime);

        float r = Input.GetAxisRaw("Mouse X");
        trans.Rotate(Vector3.up * rotateSpeed * Time.deltaTime * r);
    }
}
