using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;
    int prefabId;
    float bulletSpeed = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

    }

    //데미지수치, 관통수치 초기화 함수
    public void Init(float damage, int per, Vector3 dir, float bulletSpeed)
    {
        this.damage = damage;
        this.per = per;
        //this.prefabId = id;
        if (per >= 0)//관통이 0이면 근접, 아니면 원거리
        {
            this.bulletSpeed = bulletSpeed;
            rigid.linearVelocity = dir * bulletSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
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

    void Update()
    {
        BulletType();
    }

    void BulletType()
    {
        switch (prefabId)
        {
            case 0://일반 총알
                break;
            case 1://부메랑

                break;
        }
    }

    void Bommerang()
    {
        Vector2 target = GameManager.instance.player.transform.position;
        Vector2 dirVec = target - rigid.position;
        //FixedUpdate를 사용하긴 하지만, 업데이트마다 이동할 때의 거리가 또 달라지지 않게끔 fixedTime을 곱해준다
        Vector2 nextVec = dirVec.normalized * bulletSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        rigid.linearVelocity = Vector2.zero;
    }
}
