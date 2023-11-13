using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    //�������� ������ ����
    public GameObject[] prefabs;


    //Ǯ ����� �ϴ� ����Ʈ
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ������ Ǯ�� ��� (��Ȱ��ȭ��) �ִ� ���� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {
            if (item && !item.activeSelf)  // item�� null�� �ƴϰ� Ȱ��ȭ���� �ʾҴ��� Ȯ��
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // �� ã������
        if (select == null)
        {
            // ���Ӱ� ���� �� select�� �Ҵ�
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }


    public GameObject GetBarrage(int index)
    {
        GameObject select = null;

        // ������ Ǯ�� ��� (��Ȱ��ȭ��) �ִ� ���� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {
            if (item && !item.activeSelf)  // item�� null�� �ƴϰ� Ȱ��ȭ���� �ʾҴ��� Ȯ��
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;
                pools[index].Remove(select);
                break;
            }
        }

        // �� ã������
        if (select == null)
        {
            // ���Ӱ� ���� �� select�� �Ҵ�
            select = Instantiate(prefabs[index], transform);
            //select.GetComponent<Barrage>().myPool = pools[index];
        }

        return select;
    }

}