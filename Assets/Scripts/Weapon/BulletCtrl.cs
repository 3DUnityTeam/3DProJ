using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{

    public float damage = 20.0f;
    public float force = 1000.0f;
    private Rigidbody myRD;
    // Start is called before the first frame update
    void Start()
    {
        myRD = GetComponent<Rigidbody>();
        myRD.AddRelativeForce(new Vector3(0,0,1) * force);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
