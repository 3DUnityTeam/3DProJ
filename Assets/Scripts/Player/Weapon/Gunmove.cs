using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunmove : MonoBehaviour
{
    private Transform myTR;
    public Transform sight;
    // Start is called before the first frame update
    void Start()
    {
        myTR = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        myTR.rotation = sight.rotation;
    }
}
