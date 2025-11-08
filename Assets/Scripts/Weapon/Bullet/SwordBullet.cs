using Unity.VisualScripting;
using UnityEngine;

public class SwordBullet : Bullet
{
    Sword sword;
    int count;
    void Awake()
    {
        
    }

    public override void Init(float damage, int per, Vector3 dir, float bulletSpeed)
    {
        sword = GetComponentInParent<Sword>();
        base.Init(damage, per, dir, bulletSpeed);
        this.gameObject.SetActive(true);
        count = per;
    }

    public void AnimationFinish()//animation 클립에서 실행함.
    {
        //검격의 위치를 바꿔주기 위함.
        Vector3 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
        count--;
        sword.MatchDir();
        if(count <= 0)
        {
            count = per;
            this.gameObject.SetActive(false);
        }
    }
}
