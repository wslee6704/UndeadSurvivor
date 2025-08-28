using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public bool isLive;//12에서 추가

    [Header("# Player Info")]
    public int health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    [Header("# Game Obeject")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
        uiLevelUp.Select(0);
    }

    void Update()
    {
        if (isLive) return;
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;
        if (exp == nextExp[Mathf.Min(level,nextExp.Length - 1)])//실제 레벨이 커진다면 고정
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;//유니티의 시간 배율
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}

