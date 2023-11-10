using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimManager : MonoBehaviour
{
    //���� Ÿ��
    public enum TrackType
    {
        Lock,
        Near,
        //Strong
    }
    //���� ����Ÿ��
    public TrackType type=(TrackType)0;
    //���° ��
    int count=0;
    //�ִ� �Ÿ�
    public float maxDistance;
    //�Ÿ� �̳��� ��
    public List<GameObject> mobList; // �þ߸� Ȯ���� ���

    //���� ���� Ÿ��
    public GameObject aimingTarget;
    //�ִ� ����
    //�ڻ��� ������ 1�� ������ ������
    [Range(0,1)]
    public float limitAngle=0.5f;
    //Lock���� Ÿ�� �۵� Ʈ����
    private int togle=0;
    private void Update()
    {
        //���� �������
        if (Input.GetKeyDown(KeyCode.Q))
        {
            togle = 1;
        }
        //����Ÿ�� ��ȭ
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
        //���� ��� ����
        //mobList=GameManager.instance.
        aimingTarget =IsTrackingTarget();
    }
    //���� ��� ���ϱ�
    GameObject IsTrackingTarget()
    { 
        //�÷��̾�
        GameObject ply = GameManager.instance.player.gameObject;

        //Lock Ÿ�� ����
        // ���� �� ����Ʈ
        List<GameObject> lockList=new List<GameObject>();
        //key:��, value:���� ����
        Dictionary<GameObject, float> lockDic=new Dictionary<GameObject, float>();

        //Near Ÿ�� ����
        //���� ����� ���
        GameObject nearMob=null;
        //�ּҰŸ� �ʱⰪ(�ִ밪 +1)
        float minDistance = maxDistance + 1f;

        //Strong Ÿ�� ����
        //���� ���� ���
        GameObject strongMob=null;

        //������ �� ���� �´� �� �˻�
        foreach(var mob in mobList)
        {
            //ĵ���� ���� �� 
            if (IsInLineOfSight(mob))
            {
                //���� �÷��̾��� �Ÿ�
                float distance = Vector3.Distance(mob.transform.position, ply.transform.position);
                //�ִ�Ÿ����� ���� ��
                if (distance <= maxDistance)
                {
                    //�÷��̾� ���� ����
                    float angle = IsPlayerSeenOfSight(mob);
                    //�ִ� �������� ū �ֵ��� �ѱ��
                    if (angle< limitAngle)
                    {
                        continue;
                    }

                    //���ǿ� �´� �ֵ� ����,����Ʈ ����
                    lockDic.Add(mob, angle);
                    lockList.Add(mob);

                    //�ּҰŸ����� ���� �ֵ�� �ʱ�ȭ
                    if(minDistance> distance)
                    {
                        minDistance = distance;
                        nearMob = mob;
                    }
                }
            }
        }

        //���� Ÿ�Կ� ���� �۵�
        if (type == TrackType.Lock)
        {
            //Ʈ���� �۵�
            if (togle==1)
            {
                //���� ���
                count++;
                //Ʈ���� ����
                togle = 0;
                //���� ���ǿ� ���� ���� ����
                lockList.Sort((GameObject x, GameObject y)=>
                {
                    if (lockDic[x] < lockDic[y]) return -1;
                    else if (lockDic[x] > lockDic[y]) return 1;
                    else return 0;  
                });
                //��� ���ڰ� ����Ʈ�� ũ�⺸�� ũ�� ����
                if (count >= lockList.Count)
                {
                    count = 0;
                }
                //���� ��� ����
                aimingTarget = lockList[count];
            }
            //���� ��� �ִ��� Ȯ��
            if (lockList.Contains(aimingTarget))
            {
                return lockList[count];
            }
            //���� ��������null
            return null;
        }
        else if (type == TrackType.Near)
            return nearMob;
        //else if (type == TrackType.Strong)
        //    return strongMob;
        return null;
    }
    //ĵ�������� ���� 
    bool IsInLineOfSight(GameObject targetObj)
    {
        //������ ��� ��ġ
        Transform target = targetObj.transform;
        //���� ȭ��
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
    //�÷��̾�� ���� ����
    float IsPlayerSeenOfSight(GameObject mob)
    {
        //�÷��̾� ��ġ
        Transform plyTransform = GameManager.instance.player.transform;
        //�÷��̾ ������ ���� ��ġ
        Transform focusTransform = GameManager.instance.focus.transform;
        //�� ��ġ
        Vector3 mobPos = mob.transform.position;
        //���� �÷��̾��� ����
        Vector3 relMobVec = mobPos - plyTransform.position;
        //������ �÷��̾��� ����(���� ����)-���麤�Ͱ� �� �ٲ�� ���� �����ؼ� �̷��� ��ġ
        Vector3 playerForward = focusTransform.position - plyTransform.position;
        //�κ����� ���� �������� cos����
        float dot = Vector3.Dot(playerForward.normalized, relMobVec.normalized);
        return dot;//�ڻ��ΰ�
        //����� ����
        //(+-)0.5�� 45��
        //������ �޸�
    }
}
