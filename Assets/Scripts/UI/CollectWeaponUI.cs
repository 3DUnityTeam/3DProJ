using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectWeaponUI : MonoBehaviour
{
    //���� ���� ������ 6��
    public GameObject[] WeaponObjs;
    //������Ʈ ��ư ������Ʈ
    private Button[] WeaponIcons;

    //���õ� ���� ������ 3��
    public GameObject[] CollectIcons;


    //���� ���õ� ���� �� �ؽ�Ʈ
    public Button StartButton;
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
        //0�� ��ư ������Ʈ ����
        Button button0 = CollectIcons[0].GetComponent<Button>();
        //1�� ��ư ������Ʈ ����
        Button button1 = CollectIcons[1].GetComponent<Button>();
        //2�� ��ư ������Ʈ ����
        Button button2 = CollectIcons[2].GetComponent<Button>();
        //���� ��ư ������Ʈ ��ư �Ҵ�
        WeaponIcons = new Button[WeaponObjs.Length];
        WeaponIcons[0] = WeaponObjs[0].GetComponent<Button>();
        WeaponIcons[1] = WeaponObjs[1].GetComponent<Button>();
        WeaponIcons[2] = WeaponObjs[2].GetComponent<Button>();
        WeaponIcons[3] = WeaponObjs[3].GetComponent<Button>();
        WeaponIcons[4] = WeaponObjs[4].GetComponent<Button>();
        WeaponIcons[5] = WeaponObjs[5].GetComponent<Button>();

        //������ �۵����� ��ư ���
        button0.onClick.AddListener(() =>
        {
            //���� ��� �̺�Ʈ �۵�
            CancelWeapon(0);
        });
        button1.onClick.AddListener(() =>
        {
            //���� ��� �̺�Ʈ �۵�
            CancelWeapon(1);
        });
        button2.onClick.AddListener(() =>
        {
            //���� ��� �̺�Ʈ �۵�
            CancelWeapon(2);
        });
    }
    //GameManager���� �ڿ� ����Ǿ���
    private void OnEnable()
    {
        //6�� ���� �̹��� ����
        WeaponObjs[0].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[0];
        WeaponObjs[1].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[1];
        WeaponObjs[2].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[2];
        WeaponObjs[3].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[3];
        WeaponObjs[4].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[4];
        WeaponObjs[5].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[5];
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
                //���õ� ���� ��ȣ
                CollectNum[clickCount] = i;
                //���õ� ���� �̹��� �� ����Ʈ
                CollectIcons[clickCount].GetComponent<Button>().interactable = true;
                CollectIcons[clickCount].GetComponent<Image>().sprite 
                    = GameManager.instance.WeaponImages[i];
                //�� ���� ��ư ��Ȱ��ȭ
                WeaponIcons[i].interactable = false;
            }
        }
    }

    //���� ���� ��� �̺�Ʈ
    public void CancelWeapon(int i)
    {
        CollectIcons[i].GetComponent<Image>().sprite = GameManager.instance.BaseImage;
        //�����ȣ
        int WPnum = CollectNum[i];
        if (WPnum == -1)
            return;
        bool result=GameManager.instance.WeaponManager.RemoveWP((WeaponManager.WeaponType)WPnum);
        if (result)
        {
            WeaponIcons[WPnum].interactable = true;
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
        if(num== CollectNum.Length)
        {
            StartButton.interactable = true;
        }
        else
        {
            StartButton.interactable = false;
        }
        weaponNum.text = num + "/" + CollectIcons.Length;
    }
    private void OnDisable()
    {
        GameManager.instance.WeaponManager.GiveToWeapon();
    }
}
