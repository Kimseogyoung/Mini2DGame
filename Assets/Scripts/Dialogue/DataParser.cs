using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataParser : MonoBehaviour
{



    // generic 응용
    T StringToEnum<T>(string e)
    {
        return (T)Enum.Parse(typeof(T), e);
    }

    /*public Vector2 GetLine(string _CSVFileName)
    {
        csvData = Resources.Load<TextAsset>(_CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });
        string[] row = data[0].Split(new char[] { ',' });
        string[] lineValue = row[1].Split(new char[] { '-' });
        return new Vector2(float.Parse(lineValue[0]), float.Parse(lineValue[1]));
    }
    */

    public Friend[] friendsParse(string _CSVFileName)//임시용 될수도  쓴다면 처음실행에만 받아오게(저장파일있으면 거기서 받아오게(이걸안씀))
    {
        List<Friend> friendList = new List<Friend>();

        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length; i++)
        {
           
            string[] row = data[i].Split(new char[] { ',' });
            Friend newFriend = new Friend();
            newFriend.id = int.Parse(row[0]);
            newFriend.name = row[1];
            newFriend.nickName = row[2];
            newFriend.miniName = row[3];
            newFriend.friendship = int.Parse(row[4]);

            friendList.Add(newFriend);

        }
        return friendList.ToArray();
    }

    public Dictionary<int, Item> ItemParse(string _CSVFileName)
    {
        Dictionary<int, Item> itemDic = new Dictionary<int, Item>();

        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            Item newItem = new Item();
            newItem.id= Int32.Parse(row[0]);
            newItem.name = row[1];
            newItem.type = StringToEnum<ItemType>(row[2]);
            
            newItem.itemImage="Image/"+row[3];
            
            newItem.content = row[4];
            for(int j=5; j < 18; j++)
            {
                if (!row[j].Equals( ""))
                {
                    newItem.effect[j - 5] = Int32.Parse(row[j]);
                }

            }
            
            newItem.price = int.Parse(row[18].Equals("")?"0":row[18]);
            itemDic.Add(int.Parse(row[0]), newItem);
            
            DatabaseManager.Instance.typeItemDic[newItem.type].Add(newItem.id);

        }
        return itemDic;
    }
    public Dictionary<int, Event> EventInfoParse(string _CSVFileName)
    {
        Dictionary<int, Event> eventDic = new Dictionary<int, Event>();
        
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length; i++)
        {
            Debug.Log(data.Length);
            string[] row = data[i].Split(new char[] { ',' });
            Debug.Log(row[1]);
            Event newEvent = new Event();
            newEvent.eventType = StringToEnum<EventType>(row[1]);
            newEvent.fileName = row[2];
            newEvent.line2line = row[3];
            newEvent.eventName = row[4];
            newEvent.content = row[5];
            eventDic.Add(int.Parse(row[0]),newEvent);

        }
        return eventDic;
    }
    public DayInfo[] DayInfoParse(string _CSVFileName)
    {
        List<DayInfo> dayInfoList = new List<DayInfo>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });
        dayInfoList.Add(new DayInfo());
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            DayInfo dayInfo = new DayInfo();
            for(int j=0; j < 5; j++)
            {
                int tmp = row[j].Equals("") ? 0 : int.Parse(row[j]);
                switch (j)
                {
                    case 0:
                        dayInfo.day = tmp;
                        break;
                    case 1:
                        dayInfo.dayStartEventID = tmp;
                        break;
                    case 2:
                        dayInfo.dayFinishEventID = tmp;
                        break;
                    case 3:
                        dayInfo.scheduleStartEventID = tmp;
                        break;
                    case 4:
                        dayInfo.alarmEventID = tmp;
                        break;
                }
            }

            dayInfoList.Add(dayInfo);

        }
        return dayInfoList.ToArray();
    }
    public Dictionary<int, Schedule> ScheduleParse(string _CSVFileName)
    {
        Dictionary<int, Schedule> scheduleDic = new Dictionary<int, Schedule>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 2; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

           
            Schedule schedule = new Schedule();
            schedule.id = int.Parse(row[0]);
            schedule.name = row[1];
            schedule.money = int.Parse(row[2]);
            schedule.energy = int.Parse(row[3]);
            schedule.intimacy = int.Parse(row[4]);

            int[] attr = new int[11];
            for(int j=0; j<11; j++)
            {
                int tmp;
               
                if (row[5 + j].Equals(""))
                {
                    tmp = 0;
                }
                else
                {                 
                    tmp = Int32.Parse(row[5 + j]);
                }
                attr[j] = tmp;
            }
            schedule.attributeValues = attr;

            schedule.dialogueContext= row[16].Split(new char[] { '-' });
            schedule.photo= row[17].Split(new char[] { '-' });
            if (!row[18].Equals(""))
                schedule.friend = int.Parse(row[18]);

            scheduleDic.Add(int.Parse(row[0]),schedule);

        }
        return scheduleDic;
    }
    public Dialogue[] DialogueParse(string _CSVFileName,int startLine,int FinishLine)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();//대사 리스트 생성
        Debug.Log(_CSVFileName+" "+ startLine);
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });
        for(int i=startLine; i < FinishLine;)
        {
            Debug.Log(i);
            string[] row = data[i].Split(new char[] { ',' });

            Dialogue dialogue = new Dialogue();
            dialogue.name = row[1];

            List<string> sentenceList = new List<string>();
            List<string> spriteList = new List<string>();
            List<string> backSpriteList = new List<string>();

            List<Question> questionList = new List<Question>();

            
            do
            {
                Question q;
                if (row[5].ToString() != "")//question or answer일때
                {
                    if (row[6].ToString() != "")
                    {
                        q = new Question(int.Parse(row[5].ToString()), int.Parse(row[6].ToString()));
                    }
                    else
                    {
                        q = new Question(int.Parse(row[5].ToString()),0);
                    }

                }
                else
                {
                    q = new Question(0,0);
                }
                questionList.Add(q);


                
                sentenceList.Add(row[2]);
                spriteList.Add(row[3]);
                backSpriteList.Add(row[4]);

                if (!row[7].Equals(""))
                {
                    dialogue.friend = int.Parse(row[7]);
                    dialogue.friendshipPoint = int.Parse(row[8]);
                }
                if (!row[9].Equals(""))
                    dialogue.money = int.Parse(row[9]);

                for(int j=0; j<Attrs.allAttrs; j++)
                {
                    if (!row[10 + j].Equals(""))
                        dialogue.attrs[j] = int.Parse(row[10 + j]);
                }



                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }

            } while (row[0].ToString() == "");

            dialogue.sentences = sentenceList.ToArray();
            dialogue.sprites = spriteList.ToArray();
            dialogue.questions = questionList.ToArray();
            dialogue.backSprites = backSpriteList.ToArray();

            dialogueList.Add(dialogue);
          
        }
        return dialogueList.ToArray();
    }
    private void Start()
    {


    }

}
