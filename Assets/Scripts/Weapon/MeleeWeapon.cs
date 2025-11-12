using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Data.Common;
using Unity.VisualScripting.InputSystem;
public class MeleeWeapon : Weapon
{

    protected override void InitForInherit(ItemData data)
    {
        base.InitForInherit(data);
        Debug.Log("근접 무기 초기화");
    }

}

public class Shovel : MeleeWeapon
{
    void Update()
    {
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }
    protected override void InitForInherit(ItemData data)
    {
        base.InitForInherit(data);
        Debug.Log("Shovel 초기화");
        speed = data.baseSpeed * Character.WeaponSpeed;
        Disposition();//원래 순서는 Disposition 이후, ApplyGear인데 순서 맞는지 확인할 필요 있음.
    }

    protected override void LevelUpForInherit()
    {
        Disposition();//무기 타입이 근접이면 재배치
    }

    protected virtual void Disposition()
    {//무기 배치해주는 함수
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            //기존에 쓰던 총알이 있다면 그걸 갖고와 재배치
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
            // 총알 위치 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            //거리만큼 위에 배치
            bullet.Translate(bullet.up * 1.5f, Space.World);
            //근접 무기는 무조건 관통이기에 -1은 Infinity per이라는 주석을 달아준다.
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero, 0);
        }

    }
}

public class Sword : MeleeWeapon
{
    GameObject bullet;//정확히 말하면 참격
    protected override void InitForInherit(ItemData data)
    {
        base.InitForInherit(data);
        Debug.Log("Sword 초기화");
        speed = data.baseSpeed * Character.WeaponRate; //스피드는 연사속도임
        bullet = Resources.Load<GameObject>("Prefabs/Bullet 3");
        if (bullet != null)
        {
            bullet = Instantiate(bullet, Vector3.zero, Quaternion.identity);
            bullet.transform.parent = transform;
            bullet.SetActive(false);
        }
        else
        {
            Debug.LogError("Prefab을 찾을 수 없음!");
        }

    }
    void Update()
    {
        if (!GameManager.instance.isLive) return;

        if (!bullet.activeSelf)
            timer += Time.deltaTime;

        if (timer > speed)
        {
            timer = 0f;
            Swing();
        }
    }
    void Swing()
    {
        //검격 위치 초기화
        MatchDir();
        bullet.GetComponent<SwordBullet>().Init(damage, count, Vector3.zero, 0f);
    }
    public void MatchDir()
    {
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localRotation = Quaternion.identity;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //마우스 포지션이 0,0이기 때문에 플레이어 벡터를 빼준다

        Vector2 inputVec = (mouseWorldPos - GameManager.instance.player.transform.position).normalized; // (1,0) 같은 벡터
        Debug.Log(inputVec.ToString());
        float angle = Mathf.Atan2(inputVec.y, inputVec.x) * Mathf.Rad2Deg; // 라디안을 도로로 변환
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        //bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, player.inputVec);
        bullet.transform.Translate(bullet.transform.up * -1.5f, Space.World);
    }

}

public class Spear : MeleeWeapon
{
    GameObject bullet;
    Image itemGauage;
    float curTime = 0;
    [Range(0f, 1f)]
    float guageFill = 0;
    protected override void InitForInherit(ItemData data)
    {
        base.InitForInherit(data);
        Debug.Log("Spear 초기화");
        speed = data.baseSpeed * Character.WeaponRate; //스피드는 연사속도임
        bullet = Resources.Load<GameObject>("Prefabs/Bullet 4");
        //창 오브젝트 초기화
        if (bullet != null)
        {
            bullet = Instantiate(bullet, Vector3.zero, Quaternion.identity);
            bullet.transform.parent = transform;
            bullet.SetActive(false);
        }
        else
        {
            Debug.LogError("Prefab을 찾을 수 없음!");
        }
        //게이지 ui 초기화
        GameObject imageObj = GameObject.Find("Canvas/HUD/Guage/GuageSlider");
        GameObject guageObj = GameObject.Find("Canvas/HUD/Guage");
        if (imageObj != null)
        {
            guageObj.SetActive(true);
            imageObj.SetActive(true);
            itemGauage = imageObj.GetComponent<Image>();
            itemGauage.fillAmount = 1;
        }

    }

    void FixedUpdate()
    {

    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;

        if (!bullet.activeSelf)//무기를 안쓰고 있을때는 게이지가 안쌓임
        {

            curTime += Time.deltaTime;
            guageFill = curTime / (float)itemData.baseSpeed;
            //Debug.Log(guageFill);
            itemGauage.fillAmount = guageFill >= 1 ? 1 : guageFill;
        }
        else//무기가 켜져있는동안은, use
        {
            //켜졌을때 동안은 전체킬수가 계속 useKill에 들어갈것
        }

        if (Input.GetKeyDown(KeyCode.Space) && itemGauage.fillAmount == 1)
        {
            curTime = 0;
            itemGauage.fillAmount = 0;
            Debug.Log("돌지!");
            Dash();
        }
    }
    public void MatchDir()
    {
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localRotation = Quaternion.identity;
        //칼날 플레이어 방향으로 돌려주기
        Debug.Log(player.inputVec.ToString());
        Vector2 inputVec = player.inputVec; // (1,0) 같은 벡터
        float angle = Mathf.Atan2(inputVec.y, inputVec.x) * Mathf.Rad2Deg; // 라디안을 도로로 변환
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle + 270);

        //bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, player.inputVec);
        bullet.transform.Translate(bullet.transform.up * 1.5f, Space.World);
    }
    void Dash()
    {
        //검격 위치 초기화
        MatchDir();
        bullet.GetComponent<Bullet>().Init(damage, count, Vector3.zero, 0f);
        bullet.SetActive(true);
        player.StartDash(bulletSpeed);
        StartCoroutine(BulletCoroutine());
    }
    private IEnumerator BulletCoroutine()
    {
        float timer = 0f;
        while (timer < bulletSpeed)
        {
            timer += Time.fixedDeltaTime;
            yield return null;
        }
        bullet.SetActive(false);
    }
}

public class Bonfire : MeleeWeapon
{

    void Update()
    {
        if (!GameManager.instance.isLive) return;
        timer += Time.deltaTime;

        if (timer > speed)
        {
            timer = 0f;
            Disposition();
        }
    }
    protected override void InitForInherit(ItemData data)
    {
        bulletSpeed = itemData.baseBulletSpeed;
        speed = data.baseSpeed * Character.WeaponRate; //스피드는 연사속도임
    }

    void Disposition()
    {
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.GetComponent<Bullet>().Init(damage, count, Vector3.zero, bulletSpeed);
    }
}