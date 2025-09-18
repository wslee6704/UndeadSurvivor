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

    int level;
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
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

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
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;

    public int spriteType;
    public int health;
    public float speed;
}