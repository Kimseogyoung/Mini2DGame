using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Title, Start, None, Schedule, Finish, End}
[System.Serializable]
public class Egg
{
    public string name;
    public int[] attributeValues;
    public bool[] isOpenhidden;//8 9 10 
    /*

    public int tmt;//0 
    public int laziness;//1
    public int artist;//2
    public int[] isOpenhidden;*/

}


[System.Serializable]
public class GameManager : Singleton<GameManager>
{
    public State state=State.Title;

    public int day; //현재 일차

    public string playerName;
    public int money;
    public int energy;//에너지
    public int intimacy;//유대감

    public Egg egg;


    public Dictionary<string, int[]> inventory=new Dictionary<string, int[]>();//물건 가구 책

    public int[] scheduleCount;

    public Friend[] friends;// 친구
    //public Item[] items; //소유한 아이템
    public Achievement[] achievement; //달성한 업적

    public Event[] eventList;//게임 내 이벤트 목록




    [HideInInspector]
    public int[] selectedSchedule =new int[3];
    public int[] changedAttrs = new int[11];



    void Start()
    {
        inventory.Add("Normal", new int[24]);
        inventory.Add("Furniture", new int[24]);
        inventory.Add("Book", new int[24]);

    }

    public void AddAttribute(int[] p_attribute)
    {

        if (p_attribute.Length == egg.attributeValues.Length)
        {
            for (int i = 0; i < egg.attributeValues.Length; i++)
            {
                egg.attributeValues[i] += p_attribute[i];
            }
        }
       
    }


    public bool IsMoneyMinus(int value)
    {
        bool result = false;
        if (value + money < 0)
        {
            result = true;
        }
       
        return result;
    }
    public bool AddEnergy(int value)
    {
        bool result = false;
        if(value + energy > 100)
        {
            energy = 100;
            result = true;
        }
        else if(value + energy < 0)
        {
            energy = 0;
        }
        else
        {
            energy += value;
            result = true;
        }
        return result;
    }
    public bool AddIntimacy(int value)
    {
        bool result = false;
        if (value + intimacy > 100)
        {
            intimacy = 100;
            result = true;
        }
        else if (value + intimacy < 0)
        {
            intimacy = 0;
        }
        else
        {
            intimacy += value;
            result = true;
        }
        return result;
    }

}
