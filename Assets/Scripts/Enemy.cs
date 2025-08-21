using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    //프리펩을 강의 06+에서 하나로 쓰게 변경하였기 떄문에 각각의 애니메이터를 받아와야한다.
    public RuntimeAnimatorController[] animaCon;
    public Rigidbody2D target;
    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    WaitForFixedUpdate wait;
    Collider2D coll;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        coll = GetComponent<Collider2D>();
    }

    void FixedUpdate()//항상 물리적인 움직임으로 FixedUpdate 사용함
    {
        //죽었거나 맞는 상태면 이동하지 않기
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

        //방향 = 위치 차이의 정규화(Normalized) 위치 차이는 타겟 위치 - 자신의 위치
        Vector2 dirVec = target.position - rigid.position;
        //FixedUpdate를 사용하긴 하지만, 업데이트마다 이동할 때의 거리가 또 달라지지 않게끔 fixedTime을 곱해준다
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        //플레이어가 키입력 값을 더해서 이동을 하는 것이, 몬스터의 방향값을 더한 이동과 같음
        rigid.MovePosition(rigid.position + nextVec);

        //충돌해서 알까기가 일어나는건 velocity가 있다는 것임. 즉 물리 속도가 추가적인 이동을 더해주지 않기 위해 0으로 설정
        //강의와는 다르게 선형가속도가 생긴거같다?
        rigid.linearVelocity = Vector2.zero;
    }

    void LateUpdate()
    {
        //타겟의 방향보다 자신의 x가 크다면 왼쪽으로 가야하므로 flip(기존 이미지가 오른쪽을 보고 있음)
        if (isLive)
            spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()//스크립트가 다시 활성화될 때, 게임데이터로 초기화하고 실행
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
        //사망 시 비활성화한 것들 다시 활성화
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);

    }

    //게임의 에네미 관련 데이터를 받아오는 함수
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animaCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
        {
            return;
        }
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());


        if (health > 0)
        {
            //살아 있는 경우(피격 애니메이션 그리기)
            anim.SetTrigger("Hit");
        }
        else
        {
            //사망시 컴포넌트들 비활성화(풀에서 재사용하므로 다시 초기화해줘야함)
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            // ..죽는 경우
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;// 다음 한 개의 물리 프레임만큼 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    public void Dead()//애니메이션 이벤트에 의해 실행됨.
    {//데드 애니메이션 참고 바람
        gameObject.SetActive(false);
    }
}
