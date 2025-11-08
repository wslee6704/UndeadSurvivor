using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    protected Rigidbody2D rigid;
    //protected int prefabId;
    protected float bulletSpeed = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

    }

    //데미지수치, 관통수치 초기화 함수
    public virtual void Init(float damage, int per, Vector3 dir, float bulletSpeed)
    {
        this.damage = damage;
        this.per = per;
        this.bulletSpeed = bulletSpeed;
    }
}
