using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponManager : MonoBehaviour
{
    //일반 무기 쿨타임
    public float[] weaponCools;
    //저장된 무기 목록
    public List<WeaponType> collect = new List<WeaponType>();
    //생성된 일반 무기
    public GameObject[] Weapon;

    //특수 무기 쿨타임
    public float specialWeaponCools;
    //저장된 무기 목록
    public List<SpecialWeaponType> specialCollect = new List<SpecialWeaponType>();
    //생성된 특수 무기
    public GameObject SpecialWeapon;


    //일반 무기 리스트
    public enum WeaponType{ 
        WP0,
        WP1,
        WP2,
    }

    //특수 무기 리스트
    public enum SpecialWeaponType
    { 
        SP0,
        SP1,
        SP2,
    }


    //일반 목록에 저장
    public bool SaveWP(WeaponType type)
    {
        if (!collect.Contains(type) && collect.Count<3)
        {
            collect.Add(type);
            return true;
        }
        return false;
    }
    //저장된 일반 무기 목록에서 삭제
    public bool RemoveWP(WeaponType type)
    {
        if (collect.Contains(type))
        {
            collect.Remove(type);
            return true;
        }
        return false;
    }

    //특수 목록에 저장
    public bool SaveSpecialWP(SpecialWeaponType type)
    {
        if (!specialCollect.Contains(type) && specialCollect.Count < 1)
        {
            specialCollect.Add(type);
            return true;
        }
        return false;
    }
    //저장된 특수 무기 목록에서 삭제
    public bool RemoveSpecialWP(SpecialWeaponType type)
    {
        if (specialCollect.Contains(type))
        {
            specialCollect.Remove(type);
            return true;
        }
        return false;
    }

    public void GiveToWeapon()
    {
        //player 불러오기
        Player ply=GameManager.instance.player;

        //일반 무기
        weaponCools = new float[collect.Count];
        Weapon = new GameObject[collect.Count];
        //저장된 무기 생성
        for (int i=0;i<collect.Count;i++)
        {
            //무기 생성
            if (ply.CommonParent != null)
            {
                if ((int)collect[i] < ply.CommonParent.transform.childCount)
                {
                    Weapon[i] = ply.CommonParent.transform.GetChild((int)collect[i]).gameObject;
                    //무기 정보 부여
                }
            }
            else
            {
                Debug.Log("not player weapon setting");
            }
        }

        //특수 무기
        //저장된 무기 생성
        for (int i = 0; i < specialCollect.Count; i++)
        {
            //무기 생성
            if (ply.SpecialParent != null)
            {
                if ((int)specialCollect[i] < ply.SpecialParent.transform.childCount)
                {
                    SpecialWeapon = ply.SpecialParent.transform.GetChild((int)specialCollect[i]).gameObject;
                }
            }
            else
            {
                Debug.Log("not player weapon setting");
            }
        }

        GameManager.instance.UIManager.BattleUI.SetActive(true);
        //첫 번째 무기 활성화
        if (ply.CommonParent != null)
        {
            GameManager.instance.WeaponManager.Weapon[0].SetActive(true);
        }
        //특수 무기 활성화
        if (ply.SpecialParent != null)
        {
            GameManager.instance.WeaponManager.SpecialWeapon.SetActive(true);
        }
    }
    private void Update()
    {
        if (Time.timeScale == 0)
            return;

        GameObject[] WP = GameManager.instance.WeaponManager.Weapon;
        //무기 바꾸기
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; i < WP.Length; i++)
            {
                if (WP[i] == null)
                    continue;

                // 현재 WP[i]의 자식 오브젝트 수
                int childCount = WP[i].transform.childCount;

                // 모든 자식 오브젝트에 대해 활성화 또는 비활성화
                for (int j = 0; j < childCount; j++)
                {
                    Transform child = WP[i].transform.GetChild(j);
                    child.gameObject.SetActive(i == 0); // i가 0이면 활성화, 아니면 비활성화
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            for (int i = 0; i < WP.Length; i++)
            {
                if (WP[i] == null)
                    continue;

                // 현재 WP[i]의 자식 오브젝트 수
                int childCount = WP[i].transform.childCount;

                // 모든 자식 오브젝트에 대해 활성화 또는 비활성화
                for (int j = 0; j < childCount; j++)
                {
                    Transform child = WP[i].transform.GetChild(j);
                    child.gameObject.SetActive(i == 1); // i가 0이면 활성화, 아니면 비활성화
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            for (int i = 0; i < WP.Length; i++)
            {
                if (WP[i] == null)
                    continue;

                // 현재 WP[i]의 자식 오브젝트 수
                int childCount = WP[i].transform.childCount;

                // 모든 자식 오브젝트에 대해 활성화 또는 비활성화
                for (int j = 0; j < childCount; j++)
                {
                    Transform child = WP[i].transform.GetChild(j);
                    child.gameObject.SetActive(i == 2); // i가 0이면 활성화, 아니면 비활성화
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.UIManager.BattleUI.activeSelf)
        {
            //일반 무기 쿨타임 진행
            for(int i = 0; i < weaponCools.Length; i++)
            {
                if (weaponCools[i] == 0)
                {
                    StartCoroutine("CoolTime",i);
                }
            }
            //특수 무기 쿨타임 진행
            if (specialWeaponCools == 0) { 
                StartCoroutine(SpecialCoolTime());
            }
        }
    }
    //
    IEnumerator CoolTime(int i)
    {
        while (weaponCools[i] < 1)
        {
            weaponCools[i] += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator SpecialCoolTime()
    {
        while (specialWeaponCools < 1)
        {
            specialWeaponCools += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
