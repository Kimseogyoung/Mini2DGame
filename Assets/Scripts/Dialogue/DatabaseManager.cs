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

    public Dictionary<ItemType, List<int>> typeItemDic;

    public static bool isFinish= false;//정보 저장이 되었는지

    //private Vector2 line;
    //Dictionary<int, Dialogue> dialogDic = new Dictionary<int, Dialogue>();

    private void Awake()
    {
        typeItemDic = new Dictionary<ItemType, List<int>>();
        for(int i=0; i<4; i++)
        {
            Debug.Log(((ItemType)i).ToString());
            typeItemDic.Add((ItemType)i,new List<int>()); 

        }
        

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
    public string GetItemEffectString(Item item)
    {
        string str = "";
        for (int i = 0; i < 13; i++)
        {
            if (i < 2)
            {

            }
            else
            {
                if (item.effect[i] != 0)
                    str += DatabaseManager.Instance.attributeNames[i - 2] + " " + item.effect[i] + "  ";
            }

        }
        return str;
    }

}
