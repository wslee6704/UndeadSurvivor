using UnityEngine;

public class RangeBullet : Bullet
{
    public override void Init(float damage, int per, Vector3 dir, float bulletSpeed)
    {
        base.Init(damage, per, dir, bulletSpeed);
        //빵 쏘는 부분
        rigid.linearVelocity = dir * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }

        per--;
        if (per <= 0 && per >= -100)
        {
            //초기화 전에 다시 쓰게 하기 위해
            rigid.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {//총알이 화면밖으로 나가면 비활성화
        if (collision.CompareTag("Area"))
        {
            rigid.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
