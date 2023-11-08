using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponManager : MonoBehaviour
{
    public float[] weaponCools;
    //���� ����Ʈ
    public enum WeaponType{ 
        WP0,
        WP1,
        WP2,
        WP3,
        WP4,
        WP5,
    }
    //���� ����� ������, ���⸮��Ʈ�� �����ϰ� ��ġ�� �ʿ�����
    public GameObject[] WeaponPrefabs;
    //����� ���� ���
    public List<WeaponType> collect = new List<WeaponType>();
    //������ ����
    [HideInInspector]public GameObject[] Weapon;

    //��Ͽ� ����
    public bool SaveWP(WeaponType type)
    {
        if (!collect.Contains(type) && collect.Count<3)
        {
            collect.Add(type);
            return true;
        }
        return false;
    }
    //����� ��Ͽ��� ����
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
        //����� ���� ����
        for (int i=0;i<collect.Count;i++)
        {
            //player �ҷ�����
            Transform ply_trans=GameManager.instance.player.transform;
            //���� ����
            Weapon[i]= Instantiate(WeaponPrefabs[(int)collect[i]], ply_trans);
            //���� ��ġ ����(���� �ʿ�)
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
