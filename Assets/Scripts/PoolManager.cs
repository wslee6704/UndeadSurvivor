using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //.. 프리팹들을 보관하는 변수
    public GameObject[] prefabs;
    // .. 풀 담당을 하는 리스트들(프리팹을 종류별로 리스트에 담아야하니 이것도 배열)
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }

        Debug.Log(pools.Length);
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        //선택한 풀의 비활성화 된 게임오브젝트 접근
        //발견시 select에 할당
        foreach (GameObject item in pools[index]){
            if (item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        //존재하지않을시, 새롭게 생성하고 select에 할당
        if (!select)
        {//오브젝트가 null이면 true일테므로 실행됨
            //생성할 오브젝트, transform을 써줌으로써, 이걸 생성하는 부모로 들어간다
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
