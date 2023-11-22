using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectWeaponUI : MonoBehaviour
{
    //일반
    //일반 무기 선택 아이콘 3종
    public GameObject[] CommonWPObjs;
    //일반 무기 오브젝트 버튼 컴포넌트
    private Button[] CommonWeaponIcons;
    //선택된 무기 아이콘 3종
    public GameObject[] CollectCommonIcons;
    //현재 선택된 일반 무기 번호 목록
    private int[] Com_CollectNum;

    //특수
    //특수 무기 선택 아이콘 3종
    public GameObject[] SpecialWPObjs;
    //특수 무기 오브젝트 버튼 컴포넌트
    private Button[] SpecialWeaponIcons;
    //선택된 무기 아이콘 1종
    public GameObject CollectSpecialIcons;
    //현재 선택된 특수 무기 번호 목록
    private int Spe_CollectNum;


    public Button StartButton;
    private void Awake()
    {
        //무기 버튼 오브젝트 버튼 할당
        CommonWeaponIcons = new Button[CommonWPObjs.Length];
        for (int i = 0; i < CommonWPObjs.Length; i++)
        {
            CommonWeaponIcons[i] = CommonWPObjs[i].GetComponent<Button>();
        }

        //번호 목록 초기화
        Com_CollectNum = new int[CollectCommonIcons.Length];
        Com_CollectNum[0] = -1;
        Com_CollectNum[1] = -1;
        Com_CollectNum[2] = -1;

        //선택된 무기 아이콘 핸들러 - for문과 작동되지 않음
        //무기 번호 목록으로 작동
        //0번 버튼 컴포넌트 선언
        Button combutton0 = CollectCommonIcons[0].GetComponent<Button>();
        //1번 버튼 컴포넌트 선언
        Button combutton1 = CollectCommonIcons[1].GetComponent<Button>();
        //2번 버튼 컴포넌트 선언
        Button combutton2 = CollectCommonIcons[2].GetComponent<Button>();

        //리스너 작동으로 버튼 대기
        combutton0.onClick.AddListener(() =>
        {
            //무기 취소 이벤트 작동
            CancelWeapon(0);
        });
        combutton1.onClick.AddListener(() =>
        {
            //무기 취소 이벤트 작동
            CancelWeapon(1);
        });
        combutton2.onClick.AddListener(() =>
        {
            //무기 취소 이벤트 작동
            CancelWeapon(2);
        });

        SpecialWeaponIcons = new Button[SpecialWPObjs.Length];
        for (int j = 0; j < SpecialWPObjs.Length; j++)
        {
            SpecialWeaponIcons[j] = SpecialWPObjs[j].GetComponent<Button>();
        }
        //번호 목록 초기화
        Spe_CollectNum = -1;
        //특수 무기 버튼 컴포넌트 선언
        Button spebutton = CollectSpecialIcons.GetComponent<Button>();
        spebutton.onClick.AddListener(() =>
        {
            //무기 취소 이벤트 작동
            CancelSpecialWeapon(0);
        });
    }
    //GameManager생성 뒤에 적용되야함
    private void OnEnable()
    {
        //3종 무기 이미지 세팅
        for(int i = 0; i < CommonWPObjs.Length; i++)
        {
            CommonWPObjs[i].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[i];
        }
        for (int j = 0; j < SpecialWPObjs.Length; j++)
        {
            SpecialWPObjs[j].GetComponent<Image>().sprite = GameManager.instance.SpecialWeaponImages[j];
        }

    }
    //일반
    //무기 선택 이벤트
    public void CollectWeapon(int i)
    {
        //추가되면 true, 안되면 false(3개보다 많이 선택할수없음)
        bool result=GameManager.instance.WeaponManager.SaveWP((WeaponManager.WeaponType)i);
        //추가되었을때
        if (result)
        {
            GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
            //비어있는 칸 구하기 비어있으면 -1 아니면 채워짐
            int clickCount = -1;
            for (int j = 0; j < Com_CollectNum.Length; j++)
            {
                if (Com_CollectNum[j] == -1)
                {
                    clickCount = j;
                    break;
                }
            }
            //다 채워지지 않았을때
            if (clickCount != -1)
            {
                //선택된 무기 번호
                Com_CollectNum[clickCount] = i;
                //선택된 무기 이미지 및 이펙트
                CollectCommonIcons[clickCount].GetComponent<Button>().interactable = true;
                CollectCommonIcons[clickCount].GetComponent<Image>().sprite 
                    = GameManager.instance.WeaponImages[i];
                //고른 무기 버튼 비활성화
                CommonWeaponIcons[i].interactable = false;
            }
        }
    }
    //무기 선택 취소 이벤트
    public void CancelWeapon(int i)
    {
        CollectCommonIcons[i].GetComponent<Image>().sprite = GameManager.instance.BaseImage;
        //무기번호
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

    //특수
    //무기 선택 이벤트
    public void CollectSpecialWeapon(int i)
    {
        //추가되면 true, 안되면 false(3개보다 많이 선택할수없음)
        bool result = GameManager.instance.WeaponManager.SaveSpecialWP((WeaponManager.SpecialWeaponType)i);
        //추가되었을때
        if (result)
        {
            GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
            //선택된 무기 번호
            Spe_CollectNum = i;
            //선택된 무기 이미지 및 이펙트
            CollectSpecialIcons.GetComponent<Button>().interactable = true;
            CollectSpecialIcons.GetComponent<Image>().sprite= GameManager.instance.SpecialWeaponImages[i];
            //고른 무기 버튼 비활성화
            SpecialWeaponIcons[i].interactable = false;
        }
    }

    //무기 선택 취소 이벤트
    public void CancelSpecialWeapon(int i)
    {
        CollectSpecialIcons.GetComponent<Image>().sprite = GameManager.instance.BaseImage;
        //무기번호
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
