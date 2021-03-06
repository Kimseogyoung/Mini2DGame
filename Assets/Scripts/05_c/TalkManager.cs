using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; //대화 딕셔너리 생성
    Dictionary<int, Sprite> portraitData;
    public Sprite[] portraitArr;

    void Start()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }
    void GenerateData()
    {
        //Talk Data
        //Npc A:1000 
        //home:100
        talkData.Add(2000, new string[] { "안녕? 나는 대결멘토야:1", "ⓕ대결을 원하면 상대해줄게:1" });//:뒤는 portraitIndex
        talkData.Add(2001, new string[] { "이겼네, 좀 하잖아:1", "이번엔 난이도 하였어\n다음엔 호락호락하지 않을걸:1", "승리 보상으로 이걸 줄게.:1" });
        talkData.Add(2002, new string[] { "날 이기다니, 대단한데?:1", "이번엔 난이도 중이었어\n다음엔 정말 쉽게 이길 수 없을거야.:1", "승리 보상으로 이걸 줄게.:1" });
        talkData.Add(2003, new string[] { "이렇게 했는데도 진다고?:1", "이제 이 부문에서는 대결이 안되네:1", "대단해! 보상으로 이걸 줄게.:1" });
        talkData.Add(2004, new string[] { "아쉽지만 내가 이겼어:1", "다음에 또 상대해줄게.:1" });
        talkData.Add(2005, new string[] { "행동력이 부족하잖아,\n행동력 3포인트가 필요해.:1", "다음에 와.:1" });
        talkData.Add(1000, new string[] { "ⓕ돌아갈까?:0" });//:뒤는 portraitIndex
        
        talkData.Add(100, new string[] { "이건 알이다." });

        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(2000 + 0, portraitArr[1]);//1000+ (여기에 스프라이트) (4개면 0,1,2,3 네줄 작성)
        portraitData.Add(2000 + 1, portraitArr[2]);
        portraitData.Add(2000 + 2, portraitArr[3]);
        portraitData.Add(2000 + 3, portraitArr[4]);

        

    }
    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }


}
