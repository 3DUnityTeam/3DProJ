using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponManager : MonoBehaviour
{
    //�Ϲ� ���� ��Ÿ��
    public float[] weaponCools;
    //����� ���� ���
    public List<WeaponType> collect = new List<WeaponType>();
    //������ �Ϲ� ����
    public GameObject[] Weapon;

    //Ư�� ���� ��Ÿ��
    public float specialWeaponCools;
    //����� ���� ���
    public List<SpecialWeaponType> specialCollect = new List<SpecialWeaponType>();
    //������ Ư�� ����
    public GameObject SpecialWeapon;


    //�Ϲ� ���� ����Ʈ
    public enum WeaponType{ 
        WP0,
        WP1,
        WP2,
    }

    //Ư�� ���� ����Ʈ
    public enum SpecialWeaponType
    { 
        SP0,
        SP1,
        SP2,
    }


    //�Ϲ� ��Ͽ� ����
    public bool SaveWP(WeaponType type)
    {
        if (!collect.Contains(type) && collect.Count<3)
        {
            collect.Add(type);
            return true;
        }
        return false;
    }
    //����� �Ϲ� ���� ��Ͽ��� ����
    public bool RemoveWP(WeaponType type)
    {
        if (collect.Contains(type))
        {
            collect.Remove(type);
            return true;
        }
        return false;
    }

    //Ư�� ��Ͽ� ����
    public bool SaveSpecialWP(SpecialWeaponType type)
    {
        if (!specialCollect.Contains(type) && specialCollect.Count < 1)
        {
            specialCollect.Add(type);
            return true;
        }
        return false;
    }
    //����� Ư�� ���� ��Ͽ��� ����
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
        //player �ҷ�����
        Player ply=GameManager.instance.player;

        //�Ϲ� ����
        weaponCools = new float[collect.Count];
        Weapon = new GameObject[collect.Count];
        //����� ���� ����
        for (int i=0;i<collect.Count;i++)
        {
            //���� ����
            if (ply.CommonParent != null)
            {
                if ((int)collect[i] < ply.CommonParent.transform.childCount)
                {
                    Weapon[i] = ply.CommonParent.transform.GetChild((int)collect[i]).gameObject;
                    //���� ���� �ο�
                }
            }
            else
            {
                Debug.Log("not player weapon setting");
            }
        }

        //Ư�� ����
        //����� ���� ����
        for (int i = 0; i < specialCollect.Count; i++)
        {
            //���� ����
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
        //ù ��° ���� Ȱ��ȭ
        if (ply.CommonParent != null)
        {
            GameManager.instance.WeaponManager.Weapon[0].SetActive(true);
        }
        //Ư�� ���� Ȱ��ȭ
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
        //���� �ٲٱ�
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; i < WP.Length; i++)
            {
                if (WP[i] == null)
                    continue;

                // ���� WP[i]�� �ڽ� ������Ʈ ��
                int childCount = WP[i].transform.childCount;

                // ��� �ڽ� ������Ʈ�� ���� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
                for (int j = 0; j < childCount; j++)
                {
                    Transform child = WP[i].transform.GetChild(j);
                    child.gameObject.SetActive(i == 0); // i�� 0�̸� Ȱ��ȭ, �ƴϸ� ��Ȱ��ȭ
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            for (int i = 0; i < WP.Length; i++)
            {
                if (WP[i] == null)
                    continue;

                // ���� WP[i]�� �ڽ� ������Ʈ ��
                int childCount = WP[i].transform.childCount;

                // ��� �ڽ� ������Ʈ�� ���� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
                for (int j = 0; j < childCount; j++)
                {
                    Transform child = WP[i].transform.GetChild(j);
                    child.gameObject.SetActive(i == 1); // i�� 0�̸� Ȱ��ȭ, �ƴϸ� ��Ȱ��ȭ
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            for (int i = 0; i < WP.Length; i++)
            {
                if (WP[i] == null)
                    continue;

                // ���� WP[i]�� �ڽ� ������Ʈ ��
                int childCount = WP[i].transform.childCount;

                // ��� �ڽ� ������Ʈ�� ���� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
                for (int j = 0; j < childCount; j++)
                {
                    Transform child = WP[i].transform.GetChild(j);
                    child.gameObject.SetActive(i == 2); // i�� 0�̸� Ȱ��ȭ, �ƴϸ� ��Ȱ��ȭ
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.UIManager.BattleUI.activeSelf)
        {
            //�Ϲ� ���� ��Ÿ�� ����
            for(int i = 0; i < weaponCools.Length; i++)
            {
                if (weaponCools[i] == 0)
                {
                    StartCoroutine("CoolTime",i);
                }
            }
            //Ư�� ���� ��Ÿ�� ����
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
