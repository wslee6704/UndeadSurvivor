using UnityEngine;

public class MeleeWeapon : Weapon
{

    void Update()
    {
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }
    public override void Init(ItemData data)
    {
        base.Init(data);
        speed = data.baseSpeed * Character.WeaponSpeed;
        Disposition();//원래 순서는 Disposition 이후, ApplyGear인데 순서 맞는지 확인할 필요 있음.
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public override void LevelUp(float damage, int count)
    {
        base.LevelUp(damage, count);
        Disposition();//무기 타입이 근접이면 재배치
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
            bullet.Rotate(rotVec);
            //거리만큼 위에 배치
            bullet.Translate(bullet.up * 1.5f, Space.World);
            //근접 무기는 무조건 관통이기에 -1은 Infinity per이라는 주석을 달아준다.
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero, 0);
        }

    }
}