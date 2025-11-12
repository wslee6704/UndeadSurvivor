using System.Threading;
using UnityEngine;


/*적을 레벨에 맞게 스폰합니다.
//스테이지에 맞는 데이터를 통해 enemy 스폰의 초기화도 여기서 작업합니다.
//스테이지 레벨은, 스폰 데이터의 길이 혹은 MaxGameTime을 일정수로 나눠진 수중
작은 수를 사용합니다
스폰 데이터가 짧아서 아마 2레벨이 최대일듯

*/
public class Spawner : MonoBehaviour
{
    //

    public Transform[] spawnPoint;
    //
    public SpawnData[] spawnData;

    public float levelTime;

    int level = 0;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();//이때 이 children은 자기 자신도 포함이 되니 인지 
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;
        timer += Time.deltaTime;
        //소수점을 버리는 함수, 소수점을 올리는 함수는 CeilToInt이다

        //현재 레벨이 계산했을 때의 레벨 보다 낮으면 Up
        if (level >= Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 5f), spawnData.Length - 1))
        {

        }
        else
        {
            level++;
            Spawn();
        }
        
        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }
    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        //GetComponentsInChildren은 자기 자신까지 포함하기 때문에 범위를 1로함
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].transform.position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
        //패턴이 활성화가 되있다면
        if (spawnData[level].bossPattern != BossPatternType.None)
        {
            Boss boss = enemy.AddComponent<Boss>();
            //플래그를 넣어서 보스 패턴 초기화
            //다만 Start에서 패턴 배열을 초기화해주기 때문에 이게 효율적인지는 좀이따 체크
            boss.PatternInit((int)spawnData[level].bossPattern);
        }
    }

    
}

[System.Flags]
public enum BossPatternType
{
    None = 0,
    PatternOfShot1 = 1 << 0,
    PatternOfShot2 = 1 << 1,
    PatternOfDash1 = 1 << 2
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;

    public bool isBoss;

    //[EnumFlags] // 커스텀 속성 (아래 코드에 정의)
    public BossPatternType bossPattern;
}

