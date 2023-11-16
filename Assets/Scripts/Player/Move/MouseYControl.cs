using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseYControl : MonoBehaviour
{
    private Transform myTR;
    private Vector3 mouseR;
    private float mouseRmingamdo;
    private float mouse;
    private float accumulatedInput = 0f;
    private bool inputRotate = false;
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
        //myTR.Rotate(new Vector3(-80f, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isCursorLocked) { 
            mouse = Input.GetAxis("Mouse Y");
            accumulatedInput += mouse;
            inputRotate = (accumulatedInput != 0f);
        }
        else
        {
            inputRotate = false;
        }
    }
    private void FixedUpdate()
    {
        mouseRmingamdo = GameManager.instance.player.YTurnSpeed;

        if (inputRotate)
        {
            myTR.Rotate(mouseR * accumulatedInput * Time.fixedDeltaTime * mouseRmingamdo);
            accumulatedInput = 0;
        }

        Vector3 trangle = myTR.localEulerAngles;
        trangle.x = (trangle.x > 180) ? trangle.x - 360 : trangle.x;
        trangle.x = Mathf.Clamp(trangle.x, -60, 30);
        // 자식 개체의 localrotation을 제한을 줄 쿼터니언으로 설정
        myTR.localEulerAngles = trangle;
    }
}
