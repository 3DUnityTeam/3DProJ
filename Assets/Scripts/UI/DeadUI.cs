using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeadUI : MonoBehaviour
{
    private float dead = 7;
    private float currentdead = 0f;
    bool activated = false;
    public Slider slider;
    public TextMeshProUGUI seconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    private void OnEnable()
    {
        currentdead = 0f;
    }
    void Update()
    {

        currentdead += Time.deltaTime;
        Mathf.Clamp(currentdead, 0, dead);
        int second = Mathf.FloorToInt(dead - currentdead);
        slider.value = currentdead / dead;
        seconds.text = second + "초 뒤에 부활...";
    }

    public void Dead(float deadtime)
    {
        dead = deadtime;
        currentdead = 0;
    }
}
