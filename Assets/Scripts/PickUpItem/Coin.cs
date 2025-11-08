using UnityEngine;

public class Coin : MonoBehaviour
{
    int expPoint = 0;
    Collider2D col;
    SpriteRenderer spriteRenderer;
    public Sprite[] coinType;
    bool magnetOn = false;

    public Rigidbody2D target;//플레이어 자석기능위해 플레이어 강체 받기
    Rigidbody2D rigid;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void MagnetEnable()
    {
        magnetOn = true;
    }

    void FixedUpdate()
    {
        if (this.magnetOn)
        {
            if (!GameManager.instance.isLive) return;
            //죽었거나 맞는 상태면 이동하지 않기


            //방향 = 위치 차이의 정규화(Normalized) 위치 차이는 타겟 위치 - 자신의 위치
            Vector2 dirVec = target.position - rigid.position;
            //FixedUpdate를 사용하긴 하지만, 업데이트마다 이동할 때의 거리가 또 달라지지 않게끔 fixedTime을 곱해준다
            Vector2 nextVec = dirVec.normalized * 7 * Time.fixedDeltaTime;

            //플레이어가 키입력 값을 더해서 이동을 하는 것이, 몬스터의 방향값을 더한 이동과 같음
            rigid.MovePosition(rigid.position + nextVec);

            //충돌해서 알까기가 일어나는건 velocity가 있다는 것임. 즉 물리 속도가 추가적인 이동을 더해주지 않기 위해 0으로 설정
            //강의와는 다르게 선형가속도가 생긴거같다?
            rigid.linearVelocity = Vector2.zero;
        }
    }

    public void Init(int hp)
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        //적의 hp받아와서 적정량으로 계산할듯
        expPoint = hp / 5;
        if (expPoint <= 4)
        {
            spriteRenderer.sprite = coinType[0];
        }
        else if (expPoint <= 6)
        {
            spriteRenderer.sprite = coinType[1];
        }
        else
        {
            spriteRenderer.sprite = coinType[2];
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        magnetOn = false;
        GameManager.instance.GetExp(expPoint);
        gameObject.SetActive(false);

    }
}
