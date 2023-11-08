using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ces_FollowCamera : MonoBehaviour
{
    Transform target;
    Transform trans;
    Transform ply;
    private void Start()
    {
        trans = GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        ply = GameManager.instance.player.transform;
        target = GameManager.instance.focus.GetComponent<Transform>();
        trans.position = ply.position+(ply.position-target.position)+new Vector3(0,3,0);
        trans.LookAt(target);
    }
}
