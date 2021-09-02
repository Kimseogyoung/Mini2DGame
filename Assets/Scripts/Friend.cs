using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FriendState
{
    None, Neighbor, Friend
}


[System.Serializable]

public class Friend
{
    public string nickName;
    public string name;
    public string miniName;
    public int id;
    public int friendship = 0;  //~30, ~80, 

    public FriendState state = FriendState.None;
    public void AddFriendship(int value)
    {
        if (friendship + value >= 80)
        {
            friendship = Mathf.Min(friendship + value, 100);
            state = FriendState.Friend;
        }
        else if (friendship + value >= 30)
        {
            friendship += value;
            state = FriendState.Neighbor;
        }
        else
        {
            friendship = Mathf.Max(friendship + value, 0);
            state = FriendState.None;
        }
    }
}
