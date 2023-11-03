using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectWeaponUI : MonoBehaviour
{
    //���� ���� ������ 6��
    public Button[] WeaponIcons;
    //���õ� ���� ������ 3��
    public GameObject[] CollectIcons;
    //���� ���õ� ���� �� �ؽ�Ʈ
    public Text weaponNum;
    //���� ���õ� ���� ��ȣ ���
    private int[] CollectNum;
    private void Awake()
    {
        //��ȣ ��� �ʱ�ȭ
        CollectNum = new int[CollectIcons.Length];
        CollectNum[0] = -1;
        CollectNum[1] = -1;
        CollectNum[2] = -1;

        //���õ� ���� ������ �ڵ鷯 - for���� �۵����� ����
        //���� ��ȣ ������� �۵�
        //��ư ������Ʈ ����
        Button button0 = CollectIcons[0].GetComponent<Button>();
        //������ �۵����� ��ư ���
        button0.onClick.AddListener(() =>
        {
            //���� ��� �̺�Ʈ �۵�
            CancelWeapon(0);
        });

        //��ư ������Ʈ ����
        Button button1 = CollectIcons[1].GetComponent<Button>();
        //������ �۵����� ��ư ���
        button1.onClick.AddListener(() =>
        {
            //���� ��� �̺�Ʈ �۵�
            CancelWeapon(1);
        });

        //��ư ������Ʈ ����
        Button button2 = CollectIcons[2].GetComponent<Button>();
        //������ �۵����� ��ư ���
        button2.onClick.AddListener(() =>
        {
            //���� ��� �̺�Ʈ �۵�
            CancelWeapon(2);
        });
    }
    //���� ���� �̺�Ʈ
    public void CollectWeapon(int i)
    {
        //�߰��Ǹ� true, �ȵǸ� false(3������ ���� �����Ҽ�����)
        bool result=GameManager.instance.WeaponManager.SaveWP((WeaponManager.WeaponType)i);
        //�߰��Ǿ�����
        if (result)
        {
            //����ִ� ĭ ���ϱ� ��������� -1 �ƴϸ� ä����
            int clickCount = -1;
            for (int j = 0; j < CollectNum.Length; j++)
            {
                if (CollectNum[j] == -1)
                {
                    clickCount = j;
                    break;
                }
            }
            //�� ä������ �ʾ�����
            if (clickCount != -1)
            {
                CollectNum[clickCount] = i;
                CollectIcons[clickCount].GetComponent<Image>().color = Color.red;
                WeaponIcons[i].interactable = false;
            }
        }
        
    }

    //���� ���� ��� �̺�Ʈ
    public void CancelWeapon(int i)
    {
        //������ ����(�ۼ� �ʿ�)
        CollectIcons[i].GetComponent<Image>().color = Color.white;

        //
        int num = CollectNum[i];
        if (num == -1)
            return;
        bool result=GameManager.instance.WeaponManager.RemoveWP((WeaponManager.WeaponType)num);
        if (result)
        {
            WeaponIcons[num].interactable = true;
            CollectNum[i]= -1;
        }
    }
    private void LateUpdate()
    {
        int num=0;
        for (int j = 0; j < CollectNum.Length; j++)
        {
            if (CollectNum[j] != -1)
            {
                num++;
            }
        }
        weaponNum.text = num + "/" + CollectIcons.Length;
    }
}
