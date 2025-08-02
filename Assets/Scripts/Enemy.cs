using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;
    bool isLive = true;

    //스프라이트의 좌우 반전을 위한 설정
    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()//항상 물리적인 움직임으로 FixedUpdate 사용함
    {
        if (!isLive) return; //죽었으면 그냥 이거 실행하지마

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
        spriter.flipX = target.position.x < rigid.position.x;
    }
}
