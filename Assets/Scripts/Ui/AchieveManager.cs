using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

//업적을 관리하는 클래스입니다.
//업적을 통한 캐릭터의 잠금 해제 로직
public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;

    public AchieveData[] achieveData;

    WaitForSecondsRealtime wait;
    public GameObject uiNotice;
    //업적 데이터와 같은 열거형 enum 생성
    enum Achieve { UnlockPotato, UnlockBean }

    //업적 데이터들을 저장해둘 배열 선언 및 초기화
    Achieve[] achieves;

    void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        //간단한 저장 기능을 제공하는 유니티 제공 클래스
        PlayerPrefs.SetInt("MyData", 1);//데이터를 초기화했는지 아닌지 확인하는 키
        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int index = 0; index < lockCharacter.Length; index++)
        {
            string achieveName = achieves[index].ToString();
            //PlayerPrefs는 업적의 달성도가 아닌, 달성 유무 체크용으로만 쓰는듯
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;

            //달성돼있으면 잠금화면은 사라지고, 캐릭터 선택창은 활성화
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach (Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        switch (achieve)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.instance.kill >= 10;
                break;
            case Achieve.UnlockBean:
                isAchieve = GameManager.instance.gameTime >= GameManager.instance.maxGameTime;
                break;
        }

        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);
            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achieve;
                if (isActive)
                {
                    GameManager.instance.notice.enqueueNoitce(achieveData[index].icon, achieveData[index].desc);
                }
                
            }
            
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
[System.Serializable]
public class AchieveData
{
    public Sprite icon;
    public String desc;
}