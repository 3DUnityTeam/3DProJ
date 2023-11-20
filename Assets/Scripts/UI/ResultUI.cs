using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    public GameObject[] CollectWeapon;
    public GameObject CollectSpecialWeapon;
    public Color[] colors;
    public TextMeshProUGUI text;
    public string[] texts;
    private void OnEnable()
    {
        //GameManager.instance.StopManager.TimeStop();
        GameManager game = GameManager.instance;
        List<WeaponManager.WeaponType> collect = game.WeaponManager.collect;
        List<WeaponManager.SpecialWeaponType> specialCollect = game.WeaponManager.specialCollect;
        for(int i = 0; i < collect.Count; i++)
        {
            CollectWeapon[i].GetComponent<Image>().sprite = game.WeaponImages[(int)collect[i]];
        }
        CollectSpecialWeapon.GetComponent<Image>().sprite = game.SpecialWeaponImages[(int)specialCollect[0]];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Textchange(bool result)
    {
        if (result == true)
        {
            text.text = texts[0];
            text.color = colors[0];
        }
        else if (result == false)
        {
            text.text = texts[1];
            text.color = colors[1];
        }
        else
        {
            return;
        }
    }
}
