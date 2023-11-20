using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    [Header("Weapon")]
    public GameObject[] CollectWeapon;
    public GameObject CollectSpecialWeapon;
    [Header("Aim")]
    public GameObject CrossHair;
    public GameObject CrossHPBar;
    public GameObject CrossHeatingBar;

    [Header("Fadeinout")]
    public FadeIn fadeIn;

    [Header("StateMassage")]
    public TextMeshProUGUI statemassage;

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
        MassageState("중간보스를 무찌르세요!");
    }

    private void FixedUpdate()
    {
        //크로스헤어 열 바
        WeaponManager WeaponManager = GameManager.instance.WeaponManager;
        GameObject weapon = WeaponManager.Weapon[WeaponManager.collectNum - 1];
        WeaponControl weaponControl = weapon.GetComponent<WeaponControl>();
        float heat = weaponControl.currentheat;
        float maxheat = weaponControl.Heat;
        if (weaponControl.state == WeaponControl.State.overheat)
        {
            CrossHeatingBar.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 150f/255f);
        }
        else
        {
            CrossHeatingBar.GetComponent<Image>().color = new Color(255f / 255f, 97f/255f,0, 150f / 255f);
        }
        CrossHeatingBar.GetComponent<Image>().fillAmount =0.2f + ((heat) / maxheat) * 0.6f;
        
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
            MobParent mob;
            if(targetMob.TryGetComponent<MobParent>(out mob))
            {
                CrossHPBar.GetComponent<Image>().fillAmount = 0.2f+((mob.MaxHP-mob.HP) / mob.MaxHP)*0.6f;
                FollowTarget(targetMob);
            }
            else
            {
                Debug.Log("Mob에 몹 부모없다");
            }
        }
        else
        {
            CrossHPBar.GetComponent<Image>().fillAmount = 0;
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

    public void MassageState(string message)
    {
        statemassage.text = message;
        StartCoroutine(HideMassage());
    }

    IEnumerator HideMassage()
    {
        yield return new WaitForSeconds(2.5f);
        statemassage.text = "";
    }
}
