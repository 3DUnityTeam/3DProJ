using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    //프리펩을 보관할 변수
    public GameObject[] prefabs;


    //풀 담당을 하는 리스트
    public List<GameObject>[] pools;

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

        // 선택한 풀의 놀고 (비활성화된) 있는 게임 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (item && !item.activeSelf)  // item이 null이 아니고 활성화되지 않았는지 확인
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 못 찾았으면
        if (select == null)
        {
            // 새롭게 생성 후 select에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }


    public GameObject GetBarrage(int index)
    {
        GameObject select = null;

        // 선택한 풀의 놀고 (비활성화된) 있는 게임 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (item && !item.activeSelf)  // item이 null이 아니고 활성화되지 않았는지 확인
            {
                // 발견하면 select 변수에 할당
                select = item;
                pools[index].Remove(select);
                break;
            }
        }

        // 못 찾았으면
        if (select == null)
        {
            // 새롭게 생성 후 select에 할당
            select = Instantiate(prefabs[index], transform);
            //select.GetComponent<Barrage>().myPool = pools[index];
        }

        return select;
    }

}