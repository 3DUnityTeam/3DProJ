using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponManager : MonoBehaviour
{
    public float[] weaponCools;
    //무기 리스트
    public enum WeaponType{ 
        WP0,
        WP1,
        WP2,
        WP3,
        WP4,
        WP5,
    }
    //무기 저장된 프리펩, 무기리스트와 동일하게 배치할 필요있음
    public GameObject[] WeaponPrefabs;
    //저장된 무기 목록
    public List<WeaponType> collect = new List<WeaponType>();
    //생성된 무기
    [HideInInspector]public GameObject[] Weapon;

    //목록에 저장
    public bool SaveWP(WeaponType type)
    {
        if (!collect.Contains(type) && collect.Count<3)
        {
            collect.Add(type);
            return true;
        }
        return false;
    }
    //저장된 목록에서 삭제
    public bool RemoveWP(WeaponType type)
    {
        if (collect.Contains(type))
        {
            collect.Remove(type);
            return true;
        }
        return false;
    }

    public void GiveToWeapon()
    {
        weaponCools = new float[collect.Count];
        Weapon = new GameObject[collect.Count];
        //저장된 무기 생성
        for (int i=0;i<collect.Count;i++)
        {
            //player 불러오기
            Transform ply_trans=GameManager.instance.player.transform;
            //무기 생성
            Weapon[i]= Instantiate(WeaponPrefabs[(int)collect[i]], ply_trans);
            //무기 위치 지정(수정 필요)
            Weapon[i].transform.position = ply_trans.position + new Vector3(0, 3, -3 + i * 3);
        }
        GameManager.instance.UIManager.BattleUI.SetActive(true);
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.UIManager.BattleUI.activeSelf)
        {
            for(int i = 0; i < weaponCools.Length; i++)
            {
                if (weaponCools[i] == 0)
                {
                    StartCoroutine("CoolTime",i);
                }
            }
        }
    }
    IEnumerator CoolTime(int i)
    {
        while (weaponCools[i] < 1)
        {
            weaponCools[i] += Time.fixedDeltaTime/10;
            yield return new WaitForFixedUpdate();
        }
        weaponCools[i] = 1;
    }
}
