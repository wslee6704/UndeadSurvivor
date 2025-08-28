using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);//비활성화도 있으므로
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        //1. 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }
        //2. 그 중에서 랜덤 3개 아이템 활성화
        int[] random = new int[3];
        while (true)
        {//함프레임에 while true를 해버리니 멈출수도있으니 조심
            random[0] = Random.Range(0, items.Length);//길이가 5이므로, 0~4까지
            random[1] = Random.Range(0, items.Length);
            random[2] = Random.Range(0, items.Length);

            if (random[0] != random[1] && random[1] != random[2] && random[0] != random[2])
                break;
        }

        for (int index = 0; index < random.Length; index++)
        {
            Item ranItem = items[random[index]];

            //3. 무기 만렙되면 소비아이템으로 대체
            if (ranItem.level == ranItem.data.damages.Length)
            {
                //소비아이템이 여러개면 여기에 RandomRange 넣고 대체해주기
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
            
        }
        
    }
}
