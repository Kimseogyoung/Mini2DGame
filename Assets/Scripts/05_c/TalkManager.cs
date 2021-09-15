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
        talkData.Add(1000, new string[] { "안녕? 나는 대결멘토야:1", "ⓕ대결을 원하면 상대해줄게:1" });//:뒤는 portraitIndex
        talkData.Add(100, new string[] { "이건 알이다." });

        portraitData.Add(1000 + 0, portraitArr[0]);//1000+ (여기에 스프라이트) (4개면 0,1,2,3 네줄 작성)
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);

        

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
