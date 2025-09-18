using System;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    LevelUp levelUp;
    ItemData data;
    int level;
    int index;
    //플레이어가 갖고 있는 Weapon과 연동되어야있어야 하므로
    public Weapon weapon;
    public Gear gear;

    //UI에 보여지기 위함. UnityEngine.UI 써줄것
    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        //첫번째 값은 자기자신이므로 두번 째 값으로 가져오기
        icon = GetComponentsInChildren<Image>()[1];
        //icon.sprite = data.itemIcon;
        
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        //textName.text = data.itemName;
        //OnEnable로 새로 해줄테니, 참고만 할수 있게끔 해주자

        //레벨업을 누를시 전체 데이터를 반영하기 위함
        levelUp = GetComponentInParent<LevelUp>();
        level = 0;
    }

    public void ChangeData(ItemData data, int index, int level)
    {
        this.data = data;
        this.index = index;
        this.level = level;
        Debug.Log(data.itemName +" 아이템 level" + level.ToString());
    }
    void OnEnable()
    {
        icon.sprite = data.itemIcon;
        textName.text = data.itemName;
        textLevel.text = "Lv." + (level + 1);
        //Desc에 쓰이는 파라미터가, 2개, 1개, 0개 다양해서 switch로 분리
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }

    }

    public void OnClick()
    {
        levelUp.LevelIncr(index);
    }

    //레벨을 LevelUp에서 전체 저장하고 있는 데이터로 넘기기
    // public void OnClick()
    // {
    //     switch (data.itemType)
    //     {
    //         case ItemData.ItemType.Melee:
    //         case ItemData.ItemType.Range:
    //             if (level == 0)
    //             {
    //                 GameObject newWeapon = new GameObject();
    //                 weapon = newWeapon.AddComponent<Weapon>();
    //                 weapon.Init(data);
    //             }
    //             else
    //             {
    //                 float nextDamage = data.baseDamage;
    //                 int nextCount = 0;

    //                 nextDamage += data.baseDamage * data.damages[level];
    //                 nextCount += data.counts[level];

    //                 weapon.LevelUp(nextDamage, nextCount);
    //             }
    //             level++;
    //             break;
    //         case ItemData.ItemType.Glove:
    //         case ItemData.ItemType.Shoe:
    //             if (level == 0)
    //             {
    //                 GameObject newGear = new GameObject();
    //                 gear = newGear.AddComponent<Gear>();
    //                 gear.Init(data);
    //             }
    //             else
    //             {
    //                 float nextRate = data.damages[level];
    //                 gear.LevelUp(nextRate);
    //             }
    //             level++;
    //             break;
    //         case ItemData.ItemType.Heal:
    //             GameManager.instance.health = GameManager.instance.maxHealth;
    //             break;
    //     }

    //     //최대레벨일 때 클릭되지 않게
    //     if (level == data.damages.Length)
    //     {
    //         GetComponent<Button>().interactable = false;
    //     }
    // }
}
