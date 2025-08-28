using System.Net.Http.Headers;
using UnityEngine;

public class Hand : MonoBehaviour
{
    //왼손 오른손 구분을 위한 변수
    public bool isLeft;
    public SpriteRenderer spriter;

    //오른손의 각 위치를 Vector3 형태로 저장
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightReversePos = new Vector3(-0.15f, -0.15f, 0);
    //왼손의 각 회적을 Quaternion 형태로 저장
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftReverseRot = Quaternion.Euler(0, 0, -135);
    SpriteRenderer player;
    void Awake()
    {
        //자신도 포함(0), 부모의 스프라이트 렌더러는 1
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;
        //왜 x, y지..?
        if (isLeft)//왼손 근접무기
        {
            transform.localRotation = isReverse ? leftReverseRot : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else//오른손 원거리 무기
        {
            transform.localPosition = isReverse ? rightReversePos : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
