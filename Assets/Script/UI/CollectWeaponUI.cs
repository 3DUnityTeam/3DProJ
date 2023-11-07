using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectWeaponUI : MonoBehaviour
{
    //무기 선택 아이콘 6종
    public GameObject[] WeaponObjs;
    //오브젝트 버튼 컴포넌트
    private Button[] WeaponIcons;

    //선택된 무기 아이콘 3종
    public GameObject[] CollectIcons;


    //현재 선택된 무기 수 텍스트
    public Button StartButton;
    public Text weaponNum;
    //현재 선택된 무기 번호 목록
    private int[] CollectNum;
    private void Awake()
    {
        //번호 목록 초기화
        CollectNum = new int[CollectIcons.Length];
        CollectNum[0] = -1;
        CollectNum[1] = -1;
        CollectNum[2] = -1;

        //선택된 무기 아이콘 핸들러 - for문과 작동되지 않음
        //무기 번호 목록으로 작동
        //0번 버튼 컴포넌트 선언
        Button button0 = CollectIcons[0].GetComponent<Button>();
        //1번 버튼 컴포넌트 선언
        Button button1 = CollectIcons[1].GetComponent<Button>();
        //2번 버튼 컴포넌트 선언
        Button button2 = CollectIcons[2].GetComponent<Button>();
        //무기 버튼 오브젝트 버튼 할당
        WeaponIcons = new Button[WeaponObjs.Length];
        WeaponIcons[0] = WeaponObjs[0].GetComponent<Button>();
        WeaponIcons[1] = WeaponObjs[1].GetComponent<Button>();
        WeaponIcons[2] = WeaponObjs[2].GetComponent<Button>();
        WeaponIcons[3] = WeaponObjs[3].GetComponent<Button>();
        WeaponIcons[4] = WeaponObjs[4].GetComponent<Button>();
        WeaponIcons[5] = WeaponObjs[5].GetComponent<Button>();

        //리스너 작동으로 버튼 대기
        button0.onClick.AddListener(() =>
        {
            //무기 취소 이벤트 작동
            CancelWeapon(0);
        });
        button1.onClick.AddListener(() =>
        {
            //무기 취소 이벤트 작동
            CancelWeapon(1);
        });
        button2.onClick.AddListener(() =>
        {
            //무기 취소 이벤트 작동
            CancelWeapon(2);
        });
    }
    //GameManager생성 뒤에 적용되야함
    private void OnEnable()
    {
        //6종 무기 이미지 세팅
        WeaponObjs[0].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[0];
        WeaponObjs[1].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[1];
        WeaponObjs[2].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[2];
        WeaponObjs[3].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[3];
        WeaponObjs[4].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[4];
        WeaponObjs[5].GetComponent<Image>().sprite = GameManager.instance.WeaponImages[5];
    }
    //무기 선택 이벤트
    public void CollectWeapon(int i)
    {
        //추가되면 true, 안되면 false(3개보다 많이 선택할수없음)
        bool result=GameManager.instance.WeaponManager.SaveWP((WeaponManager.WeaponType)i);
        //추가되었을때
        if (result)
        {
            //비어있는 칸 구하기 비어있으면 -1 아니면 채워짐
            int clickCount = -1;
            for (int j = 0; j < CollectNum.Length; j++)
            {
                if (CollectNum[j] == -1)
                {
                    clickCount = j;
                    break;
                }
            }
            //다 채워지지 않았을때
            if (clickCount != -1)
            {
                //선택된 무기 번호
                CollectNum[clickCount] = i;
                //선택된 무기 이미지 및 이펙트
                CollectIcons[clickCount].GetComponent<Button>().interactable = true;
                CollectIcons[clickCount].GetComponent<Image>().sprite 
                    = GameManager.instance.WeaponImages[i];
                //고른 무기 버튼 비활성화
                WeaponIcons[i].interactable = false;
            }
        }
    }

    //무기 선택 취소 이벤트
    public void CancelWeapon(int i)
    {
        CollectIcons[i].GetComponent<Image>().sprite = GameManager.instance.BaseImage;
        //무기번호
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
