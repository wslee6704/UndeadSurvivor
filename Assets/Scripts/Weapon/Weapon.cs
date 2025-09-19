using System.Data.Common;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;//무기의 타입 id 확인
    public int prefabId;//풀 매니저에서 참조할 때의 프리팹의 id
    public float damage;
    public int count;
    public float speed;//회전속도
    protected ItemData itemData;//무기의 타입으로 변경하기 위해


    protected float timer;
    protected Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }


    public virtual void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        //Disposition
        //레벨업시, 수치가적용되기 위해서
        
    }

    public virtual void Init(ItemData data)//이때 무기의 id에 따라 초기화가 달라져야함
    {
        //Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;//부모 지정
        transform.localPosition = Vector3.zero; //플레이어의 자식이니 로컬 위치의 제로로

        //Property Set
        itemData = data;
        //좀 바보같다 이미 data를 받아와서
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            //프리팹 아이디는 풀링 매니저의 변수에서 찾아서 초기화
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }



        //Hand set
        //public enum ItemType { Melee, Range, Glove, Shoe, Heal }
        //int로 강제 형변환해주면 자연스럽게 0,1로 바뀜
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        //원래 applyGear 하는 자리.
    }

    

    
}



