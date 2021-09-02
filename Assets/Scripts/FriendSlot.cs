using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendSlot : MonoBehaviour
{
    private int slotNum;
    public Phone phone;

    public TextMeshProUGUI text_name;
    public Image friendshipImg;
    public GameObject btn_message;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetFriend(int num,Friend friend)
    {
        slotNum = num;
        if (friend.state == FriendState.None)
        {
            text_name.text = friend.nickName;
        }
        else
        {
            text_name.text = friend.nickName + "(" + friend.name + ")";
        }        
    }
}
