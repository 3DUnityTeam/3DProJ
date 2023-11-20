using UnityEngine;

public class MainKey : MonoBehaviour
{
    public GameObject[] pressed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            pressed[0].SetActive(true);
        if (Input.GetKeyUp(KeyCode.Alpha1))
            pressed[0].SetActive(false);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            pressed[1].SetActive(true);
        if (Input.GetKeyUp(KeyCode.Alpha2))
            pressed[1].SetActive(false);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            pressed[2].SetActive(true);
        if (Input.GetKeyUp(KeyCode.Alpha3))
            pressed[2].SetActive(false);


        if (Input.GetKeyDown(KeyCode.W))
            pressed[3].SetActive(true);
        if (Input.GetKeyUp(KeyCode.W))
            pressed[3].SetActive(false);

        if (Input.GetKeyDown(KeyCode.A))
            pressed[4].SetActive(true);
        if (Input.GetKeyUp(KeyCode.A))
            pressed[4].SetActive(false);

        if (Input.GetKeyDown(KeyCode.S))
            pressed[5].SetActive(true);
        if (Input.GetKeyUp(KeyCode.S))
            pressed[5].SetActive(false);

        if (Input.GetKeyDown(KeyCode.D))
            pressed[6].SetActive(true);
        if (Input.GetKeyUp(KeyCode.D))
            pressed[6].SetActive(false);


        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            pressed[7].SetActive(true);
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            pressed[7].SetActive(false);

        if (Input.GetKeyDown(KeyCode.Space))
            pressed[8].SetActive(true);
        if (Input.GetKeyUp(KeyCode.Space))
            pressed[8].SetActive(false);

        if (Input.GetKeyDown(KeyCode.E))
            pressed[9].SetActive(true);
        if (Input.GetKeyUp(KeyCode.E))
            pressed[9].SetActive(false);
    }
}
