using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FriendState
{
    None, Neighbor, Friend
}
public class Friend
{
    public string nickName;
    public string name;
    public int id;
    public FriendState state = FriendState.None;
    public int friendship = 0;  //~30, ~80, 
}
