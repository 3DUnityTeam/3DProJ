using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ymove : MonoBehaviour
{
    private Transform myTR;
    private float R;
    private Vector3 mouseR;
    public float turnspeed = 200f;
    // Start is called before the first frame update
    private void Awake()
    {
        myTR = GetComponent<Transform>();
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.3f);
        mouseR = Vector3.left;
    }
    // Update is called once per frame
    void Update()
    {
        R = Input.GetAxis("Mouse Y");

        myTR.Rotate(R * mouseR * turnspeed * Time.deltaTime);
    }
}
