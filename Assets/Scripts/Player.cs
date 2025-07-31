using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]public Vector2 inputVec;
    Rigidbody2D rigid;
    public float speed = 3;
    SpriteRenderer spriter;
    Animator anim;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
        
        Vector2 nextVetor = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVetor);
    }

    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);//벡터의 순수 길이값
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;//연산의 결과로 그대로 들어가게
        }
    }
    
}
