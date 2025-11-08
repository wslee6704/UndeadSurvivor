using UnityEngine;

public class BoomerangBullet : Bullet
{
    bool isReturn = false;
    Vector3 basePos;
    
    public override void Init(float damage, int per, Vector3 dir, float bulletSpeed)
    {
        base.Init(damage, per, dir, bulletSpeed);
        //빵 쏘는 부분(RangeBullet 코드와 같다)
        rigid.linearVelocity = dir * bulletSpeed;
        basePos = transform.position;
    }
    void Update()
    {
        if (GameManager.instance.isLive) this.transform.Rotate(0, 0, bulletSpeed/2);

            
        if (Vector3.SqrMagnitude(this.transform.position - basePos) > 100)
        {
            isReturn = true;
        }

    }
    void FixedUpdate()
    {
        if (isReturn)
        {
            GetBack();
        }
    }
    void GetBack()
    {
        Transform target = GameManager.instance.player.transform;
        //방향 = 위치 차이의 정규화(Normalized) 위치 차이는 타겟 위치 - 자신의 위치
        Vector2 dirVec = target.position - transform.position;
        //FixedUpdate를 사용하긴 하지만, 업데이트마다 이동할 때의 거리가 또 달라지지 않게끔 fixedTime을 곱해준다
        Vector2 nextVec = dirVec.normalized * bulletSpeed * Time.fixedDeltaTime;

        //플레이어가 키입력 값을 더해서 이동을 하는 것이, 몬스터의 방향값을 더한 이동과 같음
        rigid.MovePosition(rigid.position + nextVec);
    }
    //돌아오는 중이고, 플레이어에게 닿으면 비활성화
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isReturn)
        {
            isReturn = false;
            gameObject.SetActive(false);
        }
    }
}
