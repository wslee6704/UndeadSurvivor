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
    public List<Weapon> weapons = new List<Weapon>();
    public Gear gear;

    [Tooltip("무기 관련 데이터")]
    [SerializeField]private ItemData[] itemDatas;//인스펙터에서 초기화
    public int[] itemLevels;//Awake에서 초기화

    void Awake()
    {
        itemDatas = Resources.LoadAll<ItemData>("Data");
        itemLevels = new int[itemDatas.Length];
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);//비활성화도 있으므로
    }

    public void Show()//이게 실질적 레벨업 창 띄우기
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
        LevelIncr(7);
        LevelIncr(6);
        LevelIncr(5);
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
                    weapons.Add(CreateWeapon(data.itemType, data));
                }
                else
                {
                    UpgradeWeapon(data, level);
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

    //레벨업 됐을때, 어떤 아이템들 보여줄지 창에 띄움
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
                items[index].ChangeData(itemDatas[4], 4, itemLevels[4]);
                items[index].gameObject.SetActive(true);
            }
            else
            {
                items[index].ChangeData(itemDatas[ranIndex], ranIndex, itemLevels[ranIndex]);
                items[index].gameObject.SetActive(true);
            }

        }

    }

    bool IsItemMaxLevel(int index)//무기 만렙인지 확인시켜주는 함수
    {
        return itemLevels[index] == itemDatas[index].damages.Length;
    }

    /*-----------무기 업그레이드, 종류별로 업그레이드 할예정이라 코드 길어져서 나눔.*/

    Weapon CreateWeapon(ItemData.ItemType type, ItemData data)
    {

        Weapon weapon = null;

        switch (type)
        {
            case ItemData.ItemType.Melee:
                weapon = CreateMelee(data);
                break;
            case ItemData.ItemType.Range:
                weapon = new GameObject().AddComponent<RangeWeapon>();
                break;
        }
        weapon.Init(data);
        return weapon;
    }

    Weapon CreateMelee(ItemData data)
    {
        GameObject newWeapon = new GameObject();
        switch (data.itemId)
        {
            case 0://삽
                return newWeapon.AddComponent<Shovel>();
            case 6://칼
                return newWeapon.AddComponent<Sword>();
            case 7://창.
                return newWeapon.AddComponent<Spear>();
            default:
                return null;
        }
    }

    // Weapon CreateRange(ItemData data)
    // {
    //     GameObject newWeapon = new GameObject();
    //     switch (data.itemId)
    //     {
    //         case 1://총
    //             return newWeapon.AddComponent<Shovel>();
    //         case 5://낫 부메랑
    //             return newWeapon.AddComponent<RangeWeapon>();
    //         default:
    //             return null;
    //     }
    // }

    void UpgradeWeapon(ItemData data, int level)
    {
        float nextDamage = data.baseDamage;
        int nextCount = 0;
        float nextBulletSpeed = data.baseBulletSpeed;

        // damage 적용
        if (data.damages != null && level < data.damages.Length)
        {
            nextDamage += data.baseDamage * data.damages[level];
        }

        // count 적용
        if (data.counts != null && level < data.counts.Length)
        {
            nextCount += data.counts[level];
        }

        // bulletSpeed 적용
        if (data.bulletSpeeds != null && level < data.bulletSpeeds.Length)
        {
            nextBulletSpeed += data.baseBulletSpeed * data.bulletSpeeds[level];
        }

        // 무기 리스트에서 찾아서 레벨업 적용
        for (int i = 0; i < weapons.Count; i++)
        {
            if (data.itemId == weapons[i].id)
            {
                weapons[i].LevelUp(nextDamage, nextCount, nextBulletSpeed);
            }
        }
    }


}
