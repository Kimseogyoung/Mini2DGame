using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Attrs
{
    public const int allAttrs=13;
    public const int attrs = 11;
}
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
    public int[] commuBattleLevels;


    public Dictionary<ItemType, Dictionary<int, Item>> inven = new Dictionary<ItemType, Dictionary<int, Item>>();

    public Dictionary<int, int> scheduleCount = new Dictionary<int, int>();//스케쥴 수행 횟수(초기화 x)

    public int[] friendshipPoints;// 친구우호도
    //public Item[] items; //소유한 아이템
    public Achievement[] achievement; //달성한 업적(이벤트와 다름)

    public List<int> clearEventList=new List<int>();//게임 내 달성(clear상태)한 이벤트 목록
   



    [HideInInspector]
    public int[] selectedSchedule =new int[3];
    public int[] changedAttrs = new int[11];

    void Awake()
    {
       
        //추후삭제 -
        scheduleCount.Add(1, 0);      scheduleCount.Add(2, 0);        scheduleCount.Add(3, 0);       scheduleCount.Add(4, 0);
        scheduleCount.Add(5, 0);       scheduleCount.Add(6, 0);        scheduleCount.Add(7, 0);       scheduleCount.Add(8, 0);
      scheduleCount.Add(9, 0); scheduleCount.Add(10, 0);scheduleCount.Add(11, 0);scheduleCount.Add(12, 0);scheduleCount.Add(13, 0);
        scheduleCount.Add(14, 0);scheduleCount.Add(15, 0);scheduleCount.Add(1000, 0);scheduleCount.Add(1001, 0);scheduleCount.Add(1002, 0); scheduleCount.Add(1003, 0);
        scheduleCount.Add(1004, 0);

        //

        //추삭
        commuBattleLevels = new int[4];
        friendshipPoints = new int[4];
        for (int i = 0; i < 4; i++) friendshipPoints[i] = 1;


        inven.Add(ItemType.Normal, new Dictionary<int, Item>());
        inven.Add(ItemType.Furniture, new Dictionary<int, Item>());
        inven.Add(ItemType.Book, new Dictionary<int, Item>());
        

        //추후삭제//////////////////////////////////////////////
        if (!GameManager.Instance.inven[ItemType.Normal].ContainsKey(1))
        {
            GameManager.Instance.inven[ItemType.Normal].Add(1, DatabaseManager.Instance.ItemsDic[1]);
            inven[ItemType.Normal][1].count = 3;
        }
        if (!GameManager.Instance.inven[ItemType.Normal].ContainsKey(4))
        {

            GameManager.Instance.inven[ItemType.Normal].Add(4, DatabaseManager.Instance.ItemsDic[4]);
        }
        if (!GameManager.Instance.inven[ItemType.Book].ContainsKey(1000))
        {

            GameManager.Instance.inven[ItemType.Book].Add(1000, DatabaseManager.Instance.ItemsDic[1000]);
        }
        if (!GameManager.Instance.inven[ItemType.Furniture].ContainsKey(200))
        {

            GameManager.Instance.inven[ItemType.Furniture].Add(200, DatabaseManager.Instance.ItemsDic[200]);
            GameManager.Instance.inven[ItemType.Furniture][200].active = true;
        }
        if (!GameManager.Instance.inven[ItemType.Furniture].ContainsKey(300))
        {

            GameManager.Instance.inven[ItemType.Furniture].Add(300, DatabaseManager.Instance.ItemsDic[300]);
            GameManager.Instance.inven[ItemType.Furniture][300].active = true;
        }
        if (!GameManager.Instance.inven[ItemType.Furniture].ContainsKey(100))
        {

            GameManager.Instance.inven[ItemType.Furniture].Add(100, DatabaseManager.Instance.ItemsDic[100]);
            GameManager.Instance.inven[ItemType.Furniture][100].active = true;
        }
        if (!GameManager.Instance.inven[ItemType.Furniture].ContainsKey(101))
        {

            GameManager.Instance.inven[ItemType.Furniture].Add(101, DatabaseManager.Instance.ItemsDic[101]);
        }
    }

    public void AddAllAttributes(int[] p_attribute, int plus = 1)
    {
        int[] arr = new int[Attrs.attrs];
        AddEnergy(plus*p_attribute[0]);
        AddIntimacy(plus*p_attribute[1]);
        for (int i = 0; i < Attrs.attrs; i++)
            arr[i] = plus*p_attribute[i + 2];
        AddAttribute(arr);
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
