using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectWeaponUI : MonoBehaviour
{
    //�Ϲ�
    //�Ϲ� ���� ���� ������ 3��
    public GameObject[] CommonWPObjs;
    //�Ϲ� ���� ������Ʈ ��ư ������Ʈ
    private Button[] CommonWeaponIcons;
    //���õ� ���� ������ 3��
    public GameObject[] CollectCommonIcons;
    //���� ���õ� �Ϲ� ���� ��ȣ ���
    private int[] Com_CollectNum;

    //Ư��
    //Ư�� ���� ���� ������ 3��
    public GameObject[] SpecialWPObjs;
    //Ư�� ���� ������Ʈ ��ư ������Ʈ
    private Button[] SpecialWeaponIcons;
    //���õ� ���� ������ 1��
    public GameObject CollectSpecialIcons;
    //���� ���õ� Ư�� ���� ��ȣ ���
    private int Spe_CollectNum;


    public Button StartButton;
    private void Awake()
    {
        //���� ��ư ������Ʈ ��ư �Ҵ�
        CommonWeaponIcons = new Button[CommonWPObjs.Length];
        for (int i = 0; i < CommonWPObjs.Length; i++)
        {
            CommonWeaponIcons[i] = CommonWPObjs[i].GetComponent<Button>();
        }

        //��ȣ ��� �ʱ�ȭ
        Com_CollectNum = new int[CollectCommonIcons.Length];
        Com_CollectNum[0] = -1;
        Com_CollectNum[1] = -1;
        Com_CollectNum[2] = -1;

        //���õ� ���� ������ �ڵ鷯 - for���� �۵����� ����
        //���� ��ȣ ������� �۵�
        //0�� ��ư ������Ʈ ����
        Button combutton0 = CollectCommonIcons[0].GetComponent<Button>();
        //1�� ��ư ������Ʈ ����
        Button combutton1 = CollectCommonIcons[1].GetComponent<Button>();
        //2�� ��ư ������Ʈ ����
        Button combutton2 = CollectCommonIcons[2].GetComponent<Button>();

        //������ �۵����� ��ư ���
        combutton0.onClick.AddListener(() =>
        {
            //���� ��� �̺�Ʈ �۵�
            CancelWeapon(0);
        });
        combutton1.onClick.AddListener(() =>
        {
            //���� ��� �̺�Ʈ �۵�
            CancelWeapon(1);
        });
        combutton2.onClick.AddListener(() =>
        {
            //���� ��� �̺�Ʈ �۵�
            CancelWeapon(2);
        });

        SpecialWeaponIcons = new Button[SpecialWPObjs.Length];
        for (int j = 0; j < SpecialWPObjs.Length; j++)
        {
            SpecialWeaponIcons[j] = SpecialWPObjs[j].GetComponent<Button>();
        }
        //��ȣ ��� �ʱ�ȭ
        Spe_CollectNum = -1;
        //Ư�� ���� ��ư ������Ʈ ����
        Button spebutton = CollectSpecialIcons.GetComponent<Button>();
        spebutton.onClick.AddListener(() =>
        {
            //���� ��� �̺�Ʈ �۵�
            CancelSpecialWeapon(0);
        });
    }
    //GameManager���� �ڿ� ����Ǿ���
    private void OnEnable()
    {
        //3�� ���� �̹��� ����
        for(int i = 0; i < CommonWPObjs.Length; i++)
        {
            CommonWPObjs[i].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[i];
        }
        for (int j = 0; j < SpecialWPObjs.Length; j++)
        {
            SpecialWPObjs[j].GetComponent<Image>().sprite = GameManager.instance.SpecialWeaponImages[j];
        }

    }
    //�Ϲ�
    //���� ���� �̺�Ʈ
    public void CollectWeapon(int i)
    {
        //�߰��Ǹ� true, �ȵǸ� false(3������ ���� �����Ҽ�����)
        bool result=GameManager.instance.WeaponManager.SaveWP((WeaponManager.WeaponType)i);
        //�߰��Ǿ�����
        if (result)
        {
            GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
            //����ִ� ĭ ���ϱ� ��������� -1 �ƴϸ� ä����
            int clickCount = -1;
            for (int j = 0; j < Com_CollectNum.Length; j++)
            {
                if (Com_CollectNum[j] == -1)
                {
                    clickCount = j;
                    break;
                }
            }
            //�� ä������ �ʾ�����
            if (clickCount != -1)
            {
                //���õ� ���� ��ȣ
                Com_CollectNum[clickCount] = i;
                //���õ� ���� �̹��� �� ����Ʈ
                CollectCommonIcons[clickCount].GetComponent<Button>().interactable = true;
                CollectCommonIcons[clickCount].GetComponent<Image>().sprite 
                    = GameManager.instance.WeaponImages[i];
                //�� ���� ��ư ��Ȱ��ȭ
                CommonWeaponIcons[i].interactable = false;
            }
        }
    }
    //���� ���� ��� �̺�Ʈ
    public void CancelWeapon(int i)
    {
        CollectCommonIcons[i].GetComponent<Image>().sprite = GameManager.instance.BaseImage;
        //�����ȣ
        int WPnum = Com_CollectNum[i];
        if (WPnum == -1)
            return;
        bool result=GameManager.instance.WeaponManager.RemoveWP((WeaponManager.WeaponType)WPnum);
        if (result)
        {
            GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
            CommonWeaponIcons[WPnum].interactable = true;
            Com_CollectNum[i]= -1;
        }
    }

    //Ư��
    //���� ���� �̺�Ʈ
    public void CollectSpecialWeapon(int i)
    {
        //�߰��Ǹ� true, �ȵǸ� false(3������ ���� �����Ҽ�����)
        bool result = GameManager.instance.WeaponManager.SaveSpecialWP((WeaponManager.SpecialWeaponType)i);
        //�߰��Ǿ�����
        if (result)
        {
            GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
            //���õ� ���� ��ȣ
            Spe_CollectNum = i;
            //���õ� ���� �̹��� �� ����Ʈ
            CollectSpecialIcons.GetComponent<Button>().interactable = true;
            CollectSpecialIcons.GetComponent<Image>().sprite= GameManager.instance.SpecialWeaponImages[i];
            //�� ���� ��ư ��Ȱ��ȭ
            SpecialWeaponIcons[i].interactable = false;
        }
    }

    //���� ���� ��� �̺�Ʈ
    public void CancelSpecialWeapon(int i)
    {
        CollectSpecialIcons.GetComponent<Image>().sprite = GameManager.instance.BaseImage;
        //�����ȣ
        int WPnum = Spe_CollectNum;
        if (WPnum == -1)
            return;
        bool result=GameManager.instance.WeaponManager.RemoveSpecialWP((WeaponManager.SpecialWeaponType)WPnum);
        if (result)
        {
            GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
            SpecialWeaponIcons[WPnum].interactable = true;
            Spe_CollectNum= -1;
        }
    }
    private void LateUpdate()
    {
        int num=0;
        for (int j = 0; j < Com_CollectNum.Length; j++)
        {
            if (Com_CollectNum[j] != -1)
            {
                num++;
            }
        }
        int sNum = 0;
        if (Spe_CollectNum != -1)
        {
            sNum = 1;
        }

        if(num== Com_CollectNum.Length && sNum==1)
        {
            StartButton.interactable = true;
        }
        else
        {
            StartButton.interactable = false;
        }
    }
    private void OnDisable()
    {
        GameManager.instance.WeaponManager.GiveToWeapon();
    }
}
