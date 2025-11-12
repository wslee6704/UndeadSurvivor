using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic; // Added for Action types

public class Boss : MonoBehaviour
{
    public GameObject enemyBullet;
    int enemyBulletId;
    Enemy enemy;

    float patternTimer = 0f;
    float patternInterval = 4f;

    private Action[] coroutines;
    private List<Action> usingPattern;
    Rigidbody2D rigid;
    Collider2D col;

    void Awake()
    {
        usingPattern = new List<Action>();
        // 원하는 코루틴들을 배열에 넣어줌
        coroutines = new Action[]
        {
            () => StartCoroutine(PatternOfShot1()),
            () => StartCoroutine(PatternOfShot2()),
            () => StartCoroutine(PatternOfDash1())
        };


    }

    public void PatternInit(int flagValue)
    {
        usingPattern.Clear();   
        Debug.Log($"플래그: {flagValue}로 초기화");
        for (int i = 0; i < 32; i++) // 최대 32비트 enum
        {
            if ((flagValue & (1 << i)) != 0)
            {
                if (i < coroutines.Length)
                {
                    Debug.Log($"패턴 추가: {i}번 패턴");
                    usingPattern.Add(coroutines[i]);
                }
            }
        }
    }

    void OnEnable()
    {
        enemyBullet = Resources.Load<GameObject>("Prefabs/Enemy Bullet");
        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            //프리팹 아이디는 풀링 매니저의 변수에서 찾아서 초기화
            if (enemyBullet == GameManager.instance.pool.prefabs[index])
            {
                enemyBulletId = index;
                break;
            }
        }
        Debug.Log("Boss소환");
        this.transform.localScale = Vector3.one * 3;
        //잠깐 멈추는 함수를 실행하기 위함
        enemy = GetComponent<Enemy>();
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        //지 나온다고 공지 띄우기
        StartCoroutine(AnnounceAppear());
    }

    void Update()
    {
        patternTimer += Time.deltaTime;

        if (patternTimer >= patternInterval)
        {
            patternTimer = 0f; // 타이머 초기화

            // 랜덤으로 하나 고름
            int randIndex = UnityEngine.Random.Range(0, usingPattern.Count);

            // 선택한 코루틴 실행   

            Debug.Log("패턴 실행!" + randIndex.ToString());
            usingPattern[randIndex].Invoke();
        }
    }


    IEnumerator PatternOfShot1()
    {
        Debug.Log("함수 실행 1");
        enemy.Stop();
        for (int i = 0; i < 10; i++)
        {
            float angle = 360f * i / 10f;       // 각도(도 단위)
            Vector2 dir = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),    // X
            Mathf.Sin(angle * Mathf.Deg2Rad)     // Y
            );

            Fire(dir);
            yield return new WaitForSeconds(0.1f);
        }
        enemy.move();
    }

    IEnumerator PatternOfCurse()
    {
        enemy.Stop();
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator PatternOfShot2()
    {
        Debug.Log("함수 실행 3");
        enemy.Stop();
        for (int i = 0; i < 10; i++)
        {

            Vector3 dir = GameManager.instance.player.transform.position - this.transform.position;
            dir = dir.normalized;
            Fire(dir);
            yield return new WaitForSeconds(0.1f);
        }
        enemy.move();
    }

    float dashDuration = 2f;// 돌진 지속 시간
    private float dashSpeed = 30f; // 돌진 속도
    public GameObject arrowPrefab;

    IEnumerator PatternOfDash1()
    {
        Debug.Log("대쉬 함수");
        enemy.Stop();
        int count = 4;

        col.isTrigger = true;

        while (0 < --count)
        {
            float timer = 0f;
            Vector2 dirVec = GameManager.instance.player.GetComponent<Rigidbody2D>().position - rigid.position;


            //화살표 ui 띄우고 위치, 각도
            arrowPrefab = GameManager.instance.pool.Get("Arrow");
            arrowPrefab.transform.position = this.transform.position;
            arrowPrefab.transform.rotation = Quaternion.FromToRotation(Vector2.right, dirVec);
            //돌진거리에 맞게 이미지 렌더러 사이즈 맞춰주기
            SpriteRenderer arrowSR = arrowPrefab.GetComponentInChildren<SpriteRenderer>();
            float dashDistance = dashSpeed * dashDuration;
            float spriteWidth = arrowSR.sprite.bounds.size.x;
            arrowPrefab.transform.localScale = new Vector3(dashDistance / spriteWidth / 3, 1, 1);
            arrowPrefab.SetActive(true);
            //돌진 경고 이미지 띄워주기(1초간)
            yield return new WaitForSeconds(1f);
            arrowPrefab.SetActive(false);
            while (timer < dashDuration)
            {
                // FixedUpdate 대신 여기서 MovePosition으로 돌진
                rigid.MovePosition(rigid.position + dirVec.normalized * dashSpeed * Time.fixedDeltaTime);
                timer += Time.fixedDeltaTime;
                yield return null;
            }
        }
        //주의할거는 돌진하다가 뒤지면 콜라이더 계속 꺼져있는거니깐 스포너에서 활성화 잘 시켜주자
        col.isTrigger = false;
        enemy.move();
    }

    //돌진 패턴용으로
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        GameManager.instance.player.MinusHp(30);
    }

    //방향을 지정해주면 발사함
    void Fire(Vector3 dir)
    {
        Debug.Log("쏴라!");
        Transform bullet = GameManager.instance.pool.Get(enemyBulletId).transform;
        bullet.position = transform.position;

        //지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<EnemyBullet>().Init(10, 1, dir, 4);//데미지,

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }



    IEnumerator AnnounceAppear()
    {
        // 한 프레임 기다려야 원하는 이미지가 띄워짐..
        yield return null;
        String str = "무시무시한 보스가 등장합니다..";
        Sprite image = this.GetComponent<SpriteRenderer>().sprite;
        GameManager.instance.notice.enqueueNoitce(image, str);
    }
}
