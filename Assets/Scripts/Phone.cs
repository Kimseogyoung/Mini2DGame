using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{

    public Friend[] friends;

    public Image phone;
    public TextMeshProUGUI text_notice;
    public GameObject memberGroup;
    public GameObject backPanel;
    public GameObject messagePanel;

    public Button btn_phoneOff;
    public Button btn_messagePanelOff;
    public Animator animator_phonePanel;
    public Animator animator_phoneImg;

    public  bool isEventActive = false;//이벤트가 준비되면  true ->그리고 true인채로 폰 팝업 열리면 이벤트 발생
    private FriendSlot[] friendSlot_members;
    
    private int currentAlarmEventId;
    // Start is called before the first frame update
    void Start()
    {
        btn_phoneOff.onClick.AddListener(OnClickPhoneOffButton);

        friendSlot_members = memberGroup.transform.GetComponentsInChildren<FriendSlot>();

        //여기서 day 알림 체크 
        DayInfo dayinfo = DatabaseManager.Instance.days[GameManager.Instance.day];
        Debug.Log(GameManager.Instance.day + " " + dayinfo.alarmEventID);
        if (dayinfo.alarmEventID != 0)
        {
            isEventActive = true;
            animator_phoneImg.SetBool("hasAlarm", isEventActive);
            currentAlarmEventId = dayinfo.alarmEventID;
            string str = DatabaseManager.Instance.eventInfo[dayinfo.alarmEventID].content.Replace("<br>", "\n");
            text_notice.text= str;
            

        }
        //친구 호감도별 쪽지 체크
    }
    void OnClickPhoneOffButton()
    {
        animator_phonePanel.SetTrigger("ActiveFalse");
        backPanel.SetActive(false);
    }
    public void ShowPhonePanel()
    {
        SetFriendSlots();
        animator_phonePanel.SetTrigger("ActiveTrue");
        backPanel.SetActive(true);
        if (isEventActive)
        {
            isEventActive = false;
            animator_phoneImg.SetBool("hasAlarm", isEventActive);
            DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.
                GetComponent<InteractionEvent>().GetDialogue(currentAlarmEventId));
        }
    }
    public void SetFriendSlots()
    {
        //추후 우호도별 정렬
        Friend[] friends = GameManager.Instance.friends;

        for(int i=0; i<friends.Length; i++)
        {
            friendSlot_members[i].SetFriend(i, friends[i]);
        }
    }
    public void SetNoticeText(int eventId)
    {

    }
    public void SetPhoneAlarm(bool value)
    {
        if (value)
        {

        }
        else
        {

        }

    }

}
