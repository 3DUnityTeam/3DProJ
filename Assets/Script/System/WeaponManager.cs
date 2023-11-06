using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponManager : MonoBehaviour
{
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
        //����� ���� ����
        for(int i=0;i<collect.Count;i++)
        {
            //player �ҷ�����
            Transform ply_trans=GameManager.instance.player.transform;`
            //���� ����
            GameObject weapon= Instantiate(WeaponPrefabs[(int)collect[i]], ply_trans);
            //���� ��ġ ����(���� �ʿ�)
            weapon.transform.position = ply_trans.position + new Vector3(0, 3, -3 + i * 3);
        }
    }
}
