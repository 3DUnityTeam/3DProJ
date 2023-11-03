using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //���� ����Ʈ
    public enum WeaponType{ 
        MachineGun,
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
        foreach(WeaponType ele in collect)
        {
            //player �ҷ�����
            Transform ply_trans=GameManager.instance.player.transform;
            //���� ����
            GameObject weapon= Instantiate(WeaponPrefabs[(int)ele], ply_trans);
            //���� ��ġ ����(���� �ʿ�)
            weapon.transform.position = ply_trans.position + new Vector3(0, 3, 0);
        }
    }
}
