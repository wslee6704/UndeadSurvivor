using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] public Vector2 inputVec;
    Rigidbody2D rigid;
    public float speed = 3;
    SpriteRenderer spriter;
    Animator anim;

    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
        speed *= Character.Speed;
    }

    void Update()
    {
        // inputVec.x = Input.GetAxis("Horizontal");
        // inputVec.y = Input.GetAxis("Vertical");
    }

    void OnMove(InputValue input)//이 함수는 자동완성이 안됨
    {
        inputVec = input.Get<Vector2>();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        Vector2 nextVetor = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVetor);
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive) return;
        anim.SetFloat("Speed", inputVec.magnitude);//벡터의 순수 길이값
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;//연산의 결과로 그대로 들어가게
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive) return;
        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            //플레이어가 죽으면 나머지들은 다 비활성화해야함.
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }


}
