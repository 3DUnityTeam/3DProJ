using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseYControl : MonoBehaviour
{
    private Transform myTR;
    private Vector3 mouseR;
    public float mouseRmingamdo = 600f;
    private float mouse;
    // Start is called before the first frame update

    private void Awake()
    {
        myTR = GetComponent<Transform>();
    }
    IEnumerator Start()
    {
        mouseR = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        mouseR = Vector3.left;
    }

    // Update is called once per frame
    void Update()
    {
        mouse = Input.GetAxis("Mouse Y");

        myTR.Rotate(mouseR * mouse * Time.deltaTime * mouseRmingamdo);
    }
}
