using UnityEngine;

public class ShotGunBullet : Bullet
{
    public override void Init(float damage, int per, Vector3 dir, float bulletSpeed)
    {
        base.Init(damage, per, dir, bulletSpeed);
        //빵 쏘는 부분
        rigid.linearVelocity = dir * bulletSpeed;
    }
    float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= per*0.4f)
        {
            timer = 0f;
            gameObject.SetActive(false);
            
        }
    }
}
