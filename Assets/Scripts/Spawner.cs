using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnPoint;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();//이때 이 children은 자기 자신도 포함이 되니 인지 
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.3f)
        {
            timer = 0;
            Debug.Log("Spawn");
            Spawn();
        }
    }
    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(Random.Range(0, 1));
        //GetComponentsInChildren은 자기 자신까지 포함하기 때문에 범위를 1로함
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].transform.position;
    }
}
