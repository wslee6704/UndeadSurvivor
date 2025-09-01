using UnityEngine;

public class Character : MonoBehaviour
{//함수가 아닌 속성으로 만들어줄 것임
    public static float Speed
    {
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    public static float WeaponSpeed
    {
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }
    public static float WeaponRate
    {
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
    }
    public static float Damage
    {
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }
    public static float DCount
    {
        get { return GameManager.instance.playerId == 3 ? 1f : 0f; }
    }
}
