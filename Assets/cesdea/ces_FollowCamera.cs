using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ces_FollowCamera : MonoBehaviour
{
    public Transform target;
    Transform trans;
    Transform ply;
    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        ply = GameManager.instance.player.transform;
        trans.position = ply.position+(ply.position-target.position)+new Vector3(0,3,0);
        trans.LookAt(target);
    }
}
