using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WPCoolTimeSlider : MonoBehaviour
{
    public enum WeaponType
    {
        Common,
        Special
    }
    public WeaponType type;
    public int WeaponNum;
    float heat = 0f;
    float maxheat = 1f;
    Slider slider;
    public TextMeshProUGUI childTime;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    private void FixedUpdate()
    {
        if (type == WeaponType.Common)
        {
            heat = GameManager.instance.WeaponManager.Weapon[WeaponNum].GetComponent<WeaponControl>().currentheat;
            maxheat = GameManager.instance.WeaponManager.Weapon[WeaponNum].GetComponent<WeaponControl>().Heat;
            if (true)
            {
                childTime.text = string.Format("{0:F1}", heat/maxheat * 100) + "%";
            }
            //Max 값 도달시

            slider.value = heat/maxheat;
            if (slider.value >= 1)
            {
                childTime.text = "100%";//childTime.text = string.Format("{0:F0}", GameManager.instance.WeaponManager.weaponCools[WeaponNum]);
            }
        }
        else if (type == WeaponType.Special)
        {
            WeaponControl control;
            GameObject special = GameManager.instance.WeaponManager.SpecialWeapon;
            if (special.TryGetComponent<WeaponControl>(out control))
            {
                heat = control.currentheat;
                maxheat = control.Heat;
            }
            if (true)
            {
                childTime.text = string.Format("{0:F1}", heat / maxheat * 100) + "%";
            }
            //Max 값 도달시
            slider.value = heat / maxheat;
            if (slider.value >= 1)
            {
                childTime.text = "100%";//childTime.text = string.Format("{0:F0}", GameManager.instance.WeaponManager.weaponCools[WeaponNum]);
            }
        }
    }
}
