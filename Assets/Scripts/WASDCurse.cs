using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WASDCurse : MonoBehaviour
{
    Image[] images; // 0~2까지 이미지, 인스펙터 연결
    public Sprite arrowSprite;       // 단일 화살표 스프라이트
    private Queue<KeyCode> keyQueue = new Queue<KeyCode>();
    private KeyCode[] possibleKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    void Awake()
    {
        images = this.GetComponentsInChildren<Image>();
    }
    void OnEnable()
    {
          // 랜덤으로 10개 큐에 넣기
        for (int i = 0; i < 10; i++)
        {
            KeyCode randomKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            keyQueue.Enqueue(randomKey);
        }

        UpdateUIImages();
    }


    void Update()
    {
        if (keyQueue.Count == 0) return;

        KeyCode nextKey = keyQueue.Peek();

        if (Input.GetKeyDown(nextKey))
        {
            keyQueue.Dequeue();       // 큐에서 제거
            UpdateUIImages();         // 이미지 갱신
        }
    }

    void UpdateUIImages()
    {
        KeyCode[] keysArray = keyQueue.ToArray();

        for (int i = 0; i < images.Length; i++)
        {
            if (i < keysArray.Length)
            {
                images[i].sprite = arrowSprite;                  // 하나의 스프라이트 사용
                images[i].rectTransform.localEulerAngles = GetRotation(keysArray[i]); // 방향 회전
                images[i].gameObject.SetActive(true);
            }
            else
            {
                images[i].gameObject.SetActive(false);
            }
        }
    }

    Vector3 GetRotation(KeyCode key)
    {
        // 하나의 화살표 스프라이트를 회전시켜 방향 표시
        switch (key)
        {
            case KeyCode.W: return new Vector3(0, 0, 0);      // 위
            case KeyCode.A: return new Vector3(0, 0, 90);     // 왼쪽
            case KeyCode.S: return new Vector3(0, 0, 180);    // 아래
            case KeyCode.D: return new Vector3(0, 0, -90);    // 오른쪽
            default: return Vector3.zero;
        }
    }
}
