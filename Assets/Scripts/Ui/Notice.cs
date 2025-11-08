using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
public class Notice : MonoBehaviour
{
    public GameObject uiNotice;//공지를 띄움.

    WaitForSecondsRealtime wait;//ui띄우는 시간\
    WaitForSecondsRealtime endWait;//ui 꺼졌을때의 텀

    private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    private bool isRunning = false; // 현재 실행 중 여부
    void Awake()
    {
        wait = new WaitForSecondsRealtime(5);//ui띄우는 시간
        endWait = new WaitForSecondsRealtime(0.5f);
    }

    public void enqueueNoitce(Sprite icon, String str)
    {
        Debug.Log("공지 보스 등장");
        coroutineQueue.Enqueue(NoticeRoutine(icon, str));
        
        if (!isRunning) // 실행 중이 아니면 바로 시작
        {
            StartCoroutine(RunQueue());
        }
    }
    
    private IEnumerator RunQueue()
    {
        isRunning = true;

        while (coroutineQueue.Count > 0)
        {
            IEnumerator coroutine = coroutineQueue.Dequeue();
            yield return StartCoroutine(coroutine); // 끝날 때까지 기다림
        }

        isRunning = false; // 다 끝나면 실행 중 상태 해제
    }
    IEnumerator NoticeRoutine(Sprite icon, String str)//단순히 안에 있는 notice를 띄움.
    {
        //1. 데이터 세팅
        Text text = this.GetComponentInChildren<Text>();
        text.text = str;
        this.GetComponentsInChildren<Image>()[1].sprite = icon;
        //2. 화면에 표시
        this.transform.localScale = Vector3.one;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        //3. 대기
        yield return wait;
        this.transform.localScale = Vector3.zero;
        //4. 꺼지는 시간 텀두기
        yield return endWait;
    }
}
