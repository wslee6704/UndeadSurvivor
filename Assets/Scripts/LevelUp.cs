using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;//List 때문에 추가


//레벨업하면 레벨업 ui를 띄워주고, item group을 넣어줍니다.
//
public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    //플레이어가 갖고 있는 Weapon과 연동되어야있어야 하므로
    public List<Weapon> weapons= new List<Weapon>();
    public Gear gear;

    [Tooltip("무기 관련 데이터")]
    public ItemData[] itemDatas;//인스펙터에서 초기화
    public int[] itemLevels;//Awake에서 초기화

    void Awake()
    {
        itemLevels = new int[itemDatas.Length];
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);//비활성화도 있으므로
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
    }

    public void Select(int index)
    {   
        //게임 매니저에서 처음 쓰는 레벨업 방식인데 임시방편
        LevelIncr(index);
    }

    public void LevelIncr(int index)
    {
        ItemData data = itemDatas[index];
        int level = itemLevels[index];

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)//Weapon 배열에 새로 추가함
                {
                    GameObject newWeapon = new GameObject();
                    weapons.Add(newWeapon.AddComponent<Weapon>()); 
                    weapons[weapons.Count - 1].Init(data);//웨폰의 항상 마지막 부분이 생성될테니,
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];
                    for(int i = 0; i < weapons.Count; i++)
                    {
                        if(data.itemId == weapons[i].id)//weapon을 담고 있는 배열에서 무기 id가 같은것을 레벨업
                        {
                            weapons[i].LevelUp(nextDamage, nextCount);
                        }
                    }
                    
                }
                itemLevels[index]++;
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                itemLevels[index]++;
                break;

            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }
    }

    void Next()
    {
        //1. 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }
        //2-1. 만렙인 무기는 안나오게
        int[] random = new int[3];
        //만렙이 있는지 확인해서 애초에 그 무기를 안넣으려고 했는데 오류나서 일단 보류
        //2. 그 중에서 랜덤 3개 아이템 활성화

        while (true)
        {//함프레임에 while true를 해버리니 멈출수도있으니 조심
            random[0] = UnityEngine.Random.Range(0, itemDatas.Length);//길이가 5이므로, 0~4까지
            random[1] = UnityEngine.Random.Range(0, itemDatas.Length);
            random[2] = UnityEngine.Random.Range(0, itemDatas.Length);


            //서로 같은지 확인
            if (random[0] != random[1] && random[1] != random[2] && random[0] != random[2])
                break;
        }
        for (int index = 0; index < random.Length; index++)
        {
            int ranIndex = random[index];

            //3. 무기 만렙되면 소비아이템으로 대체
            if (IsItemMaxLevel(ranIndex))
            {
                //소비아이템이 여러개면 여기에 RandomRange 넣고 대체해주기
                items[index].ChangeData(itemDatas[4],4,itemLevels[4]);
                items[index].gameObject.SetActive(true);
            }
            else
            {
                items[index].ChangeData(itemDatas[ranIndex],ranIndex,itemLevels[ranIndex]);
                items[index].gameObject.SetActive(true);
            }

        }

    }

    bool IsItemMaxLevel(int index)//무기 만렙인지 확인시켜주는 함수
    {
        return itemLevels[index] == itemDatas[index].damages.Length;
    }
}
