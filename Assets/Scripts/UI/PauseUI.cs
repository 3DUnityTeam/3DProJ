using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public GameObject ButtonBox;
    public GameObject Option;
    private void OnEnable()
    {
        GameManager.instance.isCursorLocked = false;
        ButtonBox.SetActive(true);
        Option.SetActive(false);
    }
    private void OnDisable()
    {
        GameManager.instance.isCursorLocked = true;
    }
}
