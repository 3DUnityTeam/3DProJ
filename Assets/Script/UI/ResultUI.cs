using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public GameObject[] CollectWeapon;
    private void OnEnable()
    {
        GameManager game = GameManager.instance;
        List<WeaponManager.WeaponType> collect = game.WeaponManager.collect;
        for(int i = 0; i < collect.Count; i++)
        {
            CollectWeapon[i].GetComponent<Image>().sprite = game.WeaponImages[(int)collect[i]];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
