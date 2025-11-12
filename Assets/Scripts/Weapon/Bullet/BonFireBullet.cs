using UnityEngine;

public class BonFireBullet : Bullet
{
    float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= bulletSpeed)
        {
            timer = 0f;
            gameObject.SetActive(false);
            
        }
    }
}
