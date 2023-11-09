using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimManager : MonoBehaviour
{
    //추적 타입
    public enum TrackType
    {
        Lock,
        Near,
        //Strong
    }
    //현재 추적타입
    public TrackType type=(TrackType)0;
    //몇번째 놈
    int count=0;
    //최대 거리
    public float maxDistance;
    //거리 이내의 몹
    public List<GameObject> mobList; // 시야를 확인할 대상

    //조준 중인 타겟
    public GameObject aimingTarget;
    //최대 각도
    //코사인 값으로 1에 갈수록 좁아짐
    [Range(0,1)]
    public float limitAngle=0.5f;
    //Lock추적 타입 작동 트리거
    private int togle=0;
    private void Update()
    {
        //다음 대상으로
        if (Input.GetKeyDown(KeyCode.Q))
        {
            togle = 1;
        }
        //추적타입 변화
        else if (Input.GetKeyDown(KeyCode.E))
        {
            count = 0;
            type = (TrackType)((int)type + 1);
            if ((int)type >= 2)
            {
                type = 0;
            }
        }
    }
    void FixedUpdate()
    {
        //추적 대상 저장
        //mobList=GameManager.instance.
        aimingTarget =IsTrackingTarget();
    }
    //추적 대상 구하기
    GameObject IsTrackingTarget()
    { 
        //플레이어
        GameObject ply = GameManager.instance.player.gameObject;

        //Lock 타입 변수
        // 추적 몹 리스트
        List<GameObject> lockList=new List<GameObject>();
        //key:몹, value:조건 사전
        Dictionary<GameObject, float> lockDic=new Dictionary<GameObject, float>();

        //Near 타입 변수
        //가장 가까운 대상
        GameObject nearMob=null;
        //최소거리 초기값(최대값 +1)
        float minDistance = maxDistance + 1f;

        //Strong 타입 변수
        //가장 강한 대상
        GameObject strongMob=null;

        //추적할 몹 조건 맞는 몹 검색
        foreach(var mob in mobList)
        {
            //캔버스 내의 몹 
            if (IsInLineOfSight(mob))
            {
                //몹과 플레이어의 거리
                float distance = Vector3.Distance(mob.transform.position, ply.transform.position);
                //최대거리보다 작은 몹
                if (distance <= maxDistance)
                {
                    //플레이어 시점 제한
                    float angle = IsPlayerSeenOfSight(mob);
                    //최대 각도보다 큰 애들을 넘기기
                    if (angle< limitAngle)
                    {
                        continue;
                    }

                    //조건에 맞는 애들 사전,리스트 포함
                    lockDic.Add(mob, angle);
                    lockList.Add(mob);

                    //최소거리보다 작은 애들로 초기화
                    if(minDistance> distance)
                    {
                        minDistance = distance;
                        nearMob = mob;
                    }
                }
            }
        }

        //현재 타입에 따라 작동
        if (type == TrackType.Lock)
        {
            //트리거 작동
            if (togle==1)
            {
                //다음 대상
                count++;
                //트리거 리셋
                togle = 0;
                //정렬 조건에 따라 오름 차순
                lockList.Sort((GameObject x, GameObject y)=>
                {
                    if (lockDic[x] < lockDic[y]) return -1;
                    else if (lockDic[x] > lockDic[y]) return 1;
                    else return 0;  
                });
                //대상 숫자가 리스트의 크기보다 크면 리셋
                if (count >= lockList.Count)
                {
                    count = 0;
                }
                //추적 대상 저장
                aimingTarget = lockList[count];
            }
            //추적 대상 있는지 확인
            if (lockList.Contains(aimingTarget))
            {
                return lockList[count];
            }
            //추적 대상없으면null
            return null;
        }
        else if (type == TrackType.Near)
            return nearMob;
        //else if (type == TrackType.Strong)
        //    return strongMob;
        return null;
    }
    //캔버스내의 유닛 
    bool IsInLineOfSight(GameObject targetObj)
    {
        //추적할 대상 위치
        Transform target = targetObj.transform;
        //현재 화면
        RectTransform aim = GameManager.instance.UIManager.BattleUI.GetComponent<RectTransform>();

        // 캔버스에서의 좌표를 가져오기
        Vector2 viewportPoint = Camera.main.WorldToScreenPoint(target.transform.position);

        // 캔버스 영역에서 확인
        if (RectTransformUtility.RectangleContainsScreenPoint(aim, viewportPoint))
        {
            return true;
        }

        return false;
    }
    //플레이어와 몹의 각도
    float IsPlayerSeenOfSight(GameObject mob)
    {
        //플레이어 위치
        Transform plyTransform = GameManager.instance.player.transform;
        //플레이어가 가지는 초점 위치
        Transform focusTransform = GameManager.instance.focus.transform;
        //몹 위치
        Vector3 mobPos = mob.transform.position;
        //몹과 플레이어의 벡터
        Vector3 relMobVec = mobPos - plyTransform.position;
        //초점과 플레이어의 벡터(정면 벡터)-정면벡터가 안 바뀌는 현상도 존재해서 이렇게 배치
        Vector3 playerForward = focusTransform.position - plyTransform.position;
        //두벡터의 내적 내적으로 cos추출
        float dot = Vector3.Dot(playerForward.normalized, relMobVec.normalized);
        return dot;//코사인값
        //양수면 정면
        //(+-)0.5가 45도
        //음수면 뒷면
    }
}
