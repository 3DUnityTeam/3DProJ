using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WPCoolTimeSlider : MonoBehaviour
{
    public int WeaponNum;
    float cool = 0f;
    Slider slider;
    public Text childTime;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    private void FixedUpdate()
    {
        cool = GameManager.instance.WeaponManager.weaponCools[WeaponNum];
        if (true)
        {
            childTime.text = string.Format("{0:F1}", GameManager.instance.WeaponManager.weaponCools[WeaponNum]);
        }
        //Max 값 도달시

        slider.value = cool;
        if (slider.value >= 1)
        {
            childTime.text = string.Format("{0:F0}", GameManager.instance.WeaponManager.weaponCools[WeaponNum]);
        }
    }
}
