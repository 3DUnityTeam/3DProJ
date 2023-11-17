using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [Header("Weapon")]
    public GameObject[] CollectWeapon;
    public GameObject CollectSpecialWeapon;
    [Header("Aim")]
    public GameObject CrossHair;

    [Header("Fadeinout")]
    public FadeIn fadeIn;

    RectTransform mine;
    RectTransform AimRect;
    GameObject targetMob;


    private void Start()
    {
        mine = GetComponent<RectTransform>();
        AimRect = CrossHair.GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        GameManager game = GameManager.instance;
        game.isCursorLocked = true;
        List<WeaponManager.WeaponType> collect = game.WeaponManager.collect;
        for (int i = 0; i < collect.Count; i++)
        {
            CollectWeapon[i].GetComponent<Image>().sprite = game.WeaponImages[(int)collect[i]];
        }
        WeaponManager.SpecialWeaponType specialCollect = game.WeaponManager.specialCollect[0];
        CollectSpecialWeapon.GetComponent<Image>().sprite = game.SpecialWeaponImages[(int)specialCollect];
    }

    private void FixedUpdate()
    {
        //타켓 설정 필요
        targetMob = GameManager.instance.AimManager.aimingTarget;
        if (GameManager.instance.player.gameObject.activeSelf)
        {
            CrossHair.SetActive(true);
        }
        else
        {
            CrossHair.SetActive(false);
        }
        if (targetMob != null)
        {
            FollowTarget(targetMob);
        }
        else
        {
            ResetAim();
        }
    }
    public void FollowTarget(GameObject obj)
    {
        Vector3 center = obj.GetComponent<Collider>().bounds.center;
        AimRect.position = Camera.main.WorldToScreenPoint(center);
    }
    public void ResetAim()
    {
        AimRect.anchoredPosition = Vector2.zero;
    }
}
