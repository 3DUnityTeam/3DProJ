using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectWeaponUI : MonoBehaviour
{
    //무기 선택 아이콘 6종
    public Button[] WeaponIcons;
    //선택된 무기 아이콘 3종
    public GameObject[] CollectIcons;
    //현재 선택된 무기 수 텍스트
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
        //버튼 컴포넌트 선언
        Button button0 = CollectIcons[0].GetComponent<Button>();
        //리스너 작동으로 버튼 대기
        button0.onClick.AddListener(() =>
        {
            //무기 취소 이벤트 작동
            CancelWeapon(0);
        });

        //버튼 컴포넌트 선언
        Button button1 = CollectIcons[1].GetComponent<Button>();
        //리스너 작동으로 버튼 대기
        button1.onClick.AddListener(() =>
        {
            //무기 취소 이벤트 작동
            CancelWeapon(1);
        });

        //버튼 컴포넌트 선언
        Button button2 = CollectIcons[2].GetComponent<Button>();
        //리스너 작동으로 버튼 대기
        button2.onClick.AddListener(() =>
        {
            //무기 취소 이벤트 작동
            CancelWeapon(2);
        });
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
                CollectNum[clickCount] = i;
                CollectIcons[clickCount].GetComponent<Image>().color = Color.red;
                WeaponIcons[i].interactable = false;
            }
        }
        
    }

    //무기 선택 취소 이벤트
    public void CancelWeapon(int i)
    {
        //아이콘 변경(작성 필요)
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
