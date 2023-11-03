using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject PauseUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PauseUI.activeSelf)
                {
                    Time.timeScale = 1f;
                    PauseUI.SetActive(false);
                }
                else
                {
                    Time.timeScale = 0f;
                    PauseUI.SetActive(true);
                }
            }
        }
        
    }
}
