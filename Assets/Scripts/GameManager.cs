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

public class ItemState
{
    public bool active;
    public int count;
    public ItemState(bool a, int c)
    {
        active = a;
        count = c;
    }
    public ItemState(int c)
    {
        active = false;
        count = c;
    }
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



    public Dictionary<string, Dictionary<int, Item>> inven = new Dictionary<string, Dictionary<int, Item>>();

    public int[] scheduleCount;

    public Friend[] friends;// 친구
    //public Item[] items; //소유한 아이템
    public Achievement[] achievement; //달성한 업적

    public Event[] eventList;//게임 내 이벤트 목록




    [HideInInspector]
    public int[] selectedSchedule =new int[3];
    public int[] changedAttrs = new int[11];

    void Awake()
    {

        inven.Add("Normal", new Dictionary<int, Item>());
        inven.Add("Furniture", new Dictionary<int, Item>());
        inven.Add("Book", new Dictionary<int, Item>());
        

        //추후삭제//////////////////////////////////////////////
        if (!GameManager.Instance.inven["Normal"].ContainsKey(1))
        {
            GameManager.Instance.inven["Normal"].Add(1, DatabaseManager.Instance.ItemsDic[1]);
            inven["Normal"][1].count = 3;
        }
        if (!GameManager.Instance.inven["Normal"].ContainsKey(4))
        {

            GameManager.Instance.inven["Normal"].Add(4, DatabaseManager.Instance.ItemsDic[4]);
        }
        if (!GameManager.Instance.inven["Book"].ContainsKey(1000))
        {

            GameManager.Instance.inven["Book"].Add(1000, DatabaseManager.Instance.ItemsDic[1000]);
        }
        if (!GameManager.Instance.inven["Furniture"].ContainsKey(200))
        {

            GameManager.Instance.inven["Furniture"].Add(200, DatabaseManager.Instance.ItemsDic[200]);
            GameManager.Instance.inven["Furniture"][200].active = true;
        }
        if (!GameManager.Instance.inven["Furniture"].ContainsKey(100))
        {

            GameManager.Instance.inven["Furniture"].Add(100, DatabaseManager.Instance.ItemsDic[100]);
            GameManager.Instance.inven["Furniture"][100].active = true;
        }
        if (!GameManager.Instance.inven["Furniture"].ContainsKey(101))
        {

            GameManager.Instance.inven["Furniture"].Add(101, DatabaseManager.Instance.ItemsDic[101]);
        }
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


    public bool AddMoney(int value)
    {
        bool result =true;
        if (value + money < 0)
        {
            result = false;
        }
        else
        {
            money += value;
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
