using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimManager : MonoBehaviour
{
    public enum TrackType
    {
        Lock,
        Near,
        //Strong
    }
    public TrackType type;
    public int count=0;
    public float maxDistance;
    public List<GameObject> mobList; // �þ߸� Ȯ���� ���

    public GameObject aimingTarget;
    void FixedUpdate()
    {
        //mobList=GameManager.instance.
        aimingTarget=IsTrackingTarget();
    }

    GameObject IsTrackingTarget()
    {
        GameObject ply = GameManager.instance.player.gameObject;

        List<GameObject> lockList=new List<GameObject>();
        Dictionary<GameObject, float> lockDic=new Dictionary<GameObject, float>();
        Dictionary<GameObject, float> sortDIc=new Dictionary<GameObject, float>();

        GameObject nearMob=null;
        float minDistance = maxDistance + 1f;

        GameObject strongMob=null;
        foreach(var mob in mobList)
        {
            if (IsInLineOfSight(mob))
            {
                float distance = Vector3.Distance(mob.transform.position, ply.transform.position);
                if (distance <= maxDistance)
                {
                    lockDic.Add(mob, distance);
                    lockList.Add(mob);
                    if(minDistance> distance)
                    {
                        minDistance = distance;
                        nearMob = mob;
                    }
                }
            }
        }

        if (type == TrackType.Lock)
        {
            if (lockList.Count != 0)
            {
                lockList.Sort((GameObject x, GameObject y)=>
                {
                    if (lockDic[x] < lockDic[y]) return -1;
                    else if (lockDic[x] > lockDic[y]) return 1;
                    else return 0;
                });
                if (count < lockList.Count)
                    return lockList[count];
                else
                    return lockList[lockList.Count - 1];
            }
            return aimingTarget;
        }
        else if (type == TrackType.Near)
            return nearMob;
        //else if (type == TrackType.Strong)
        //    return strongMob;
        else
            return null;
    }
    bool IsInLineOfSight(GameObject targetObj)
    {
        Transform target = targetObj.transform;
        RectTransform aim = GameManager.instance.UIManager.BattleUI.GetComponent<RectTransform>();

        // ĵ���������� ��ǥ�� ��������
        Vector2 viewportPoint = Camera.main.WorldToScreenPoint(target.transform.position);

        // ĵ���� �������� Ȯ��
        if (RectTransformUtility.RectangleContainsScreenPoint(aim, viewportPoint))
        {
            return true;
        }

        return false;
    }
}
