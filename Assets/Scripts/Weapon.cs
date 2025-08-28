using System.Data.Common;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;//무기의 타입 id 확인
    public int prefabId;//풀 매니저에서 참조할 때의 프리팹의 id
    public float damage;
    public int count;
    public float speed;//회전속도


    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
        // ..Test Code..
        if (Input.GetButtonDown("Jump")) LevelUp(damage + 1, 1);
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0) Disposition();//무기 타입이 근접이면 재배치

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)//이때 무기의 id에 따라 초기화가 달라져야함
    {
        //Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;//부모 지정
        transform.localPosition = Vector3.zero; //플레이어의 자식이니 로컬 위치의 제로로

        //Property Set
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

        switch (id)
        {
            case 0:
                speed = 150;//수치 양수에 Back하면 시계방향으로 돎
                Disposition();
                break;
            default:
                speed = 0.3f; //스피드는 연사속도임
                break;
        }

        //Hand set
        //public enum ItemType { Melee, Range, Glove, Shoe, Heal }
        //int로 강제 형변환해주면 자연스럽게 0,1로 바뀜
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    void Disposition()
    {//무기 배치해주는 함수
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            //기존에 쓰던 총알이 있다면 그걸 갖고와 재배치
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
            // 총알 위치 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            Debug.Log(bullet.position.x + " " + bullet.position.y);
            bullet.Rotate(rotVec);
            //거리만큼 위에 배치
            bullet.Translate(bullet.up * 1.5f, Space.World);
            //근접 무기는 무조건 관통이기에 -1은 Infinity per이라는 주석을 달아준다.
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }

    }

    void Fire()
    {
        //대상이 없으면 리턴
        if (!player.scanner.nearestTarget) return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        //이렇게 해야 힘을 주는 방향이 나옴.
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        //지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
