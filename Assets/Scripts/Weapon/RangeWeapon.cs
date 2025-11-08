using UnityEngine;

public class RangeWeapon : Weapon
{
    void Update()
    {
        if (!GameManager.instance.isLive) return;
        timer += Time.deltaTime;

        if (timer > speed)
        {
            timer = 0f;
            Fire();
        }
    }



    protected override void InitForInherit(ItemData data)
    {
        bulletSpeed = itemData.baseBulletSpeed;
        speed = data.baseSpeed * Character.WeaponRate; //스피드는 연사속도임
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
        bullet.GetComponent<Bullet>().Init(damage, count, dir,bulletSpeed );

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}