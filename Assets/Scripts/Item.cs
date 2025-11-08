using System;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    LevelUp levelUp;
    ItemData data;
    int level;
    int index;

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
    }
    void OnEnable()
    {
        icon.sprite = data.itemIcon;
        textName.text = data.itemName;
        textLevel.text = "Lv." + (level + 1);
        //Desc에 쓰이는 파라미터가, 2개, 1개, 0개 다양해서 switch로 분리
        textDesc.text = data.GetDescription(level);


    }

    public void OnClick()
    {
        levelUp.LevelIncr(index);
    }


}
