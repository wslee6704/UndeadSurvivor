using UnityEngine;

public class Reposition : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area"))
        {
            return;
        }

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        //방향을 아는 변수(Normalized가 없으면 이 작업 안해두 되긴함)
        Vector2 playerDir = GameManager.instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    //타일 모음 하나가 20칸이고, 그래서 2개를 뛰어넘은 거리를 이동해야하니 40 이동
                    transform.Translate(Vector3.right * dirX * 40);
                }else if(diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirX * 40);
                }
                break;
            case "Enemy":
                break;
        }
    }
}
