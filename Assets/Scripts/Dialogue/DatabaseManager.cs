using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//파싱한 데이터를 데이터베이스에서 저장 및 관
public class DatabaseManager :Singleton<DatabaseManager>//싱글톤
{
    public string[] attributeNames;
    [SerializeField]
    public Dictionary<int, Schedule> scheduleDic;
    [SerializeField]
    public DayInfo[] days;

    public Dictionary<int,string[]> eventInfo;
    public Dictionary<int, Item> ItemsDic;


    public static bool isFinish= false;//정보 저장이 되었는지

    //private Vector2 line;
    //Dictionary<int, Dialogue> dialogDic = new Dictionary<int, Dialogue>();

    private void Awake()
    {
        GetSchedule();
        //GetDialogueDataToDic();
    }

    public void GetSchedule()
    {
        DataParser parser = GetComponent<DataParser>();
        scheduleDic = parser.ScheduleParse("스케쥴");
        eventInfo = parser.EventInfoParse("이벤트정보");
        days = parser.DayInfoParse("일차정보");
        ItemsDic = parser.ItemParse("아이템 목록");

    }
    public Dialogue[] GetDialogue(string _CSVFileName, int startLine, int FinishLine)
    {
        /* int _StartNum = (int)line.x;
         int _EndNum = (int)line.y;
         Debug.Log(_EndNum + _StartNum);
         List<Dialogue> dialogueList = new List<Dialogue>();

         for (int i = 0; i <= _EndNum - _StartNum; i++)
         {
             dialogueList.Add(dialogDic[_StartNum + i]);
         }
         return dialogueList.ToArray();*/
        DataParser parser = GetComponent<DataParser>();
        Dialogue[] dialogues = parser.DialogueParse(_CSVFileName, startLine,FinishLine);

        return dialogues;
    }
    /*
    public void GetDialogueDataToDic()
    {
        DialogueParser parser = GetComponent<DialogueParser>();
        line = parser.GetLine(csv_FileName);
        Dialogue[] dialogues = parser.Parse(csv_FileName);
        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogDic.Add(i + 1, dialogues[i]);
        }
        isFinish = true;
    }
    */

}
