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
    public Animator animator;

    private GameObject[] objs_member;
    private bool isEventActive=false;//이벤트가 준비되면  true ->그리고 true인채로 폰 팝업 열리면 이벤트 발생
    private string fileName;
    private string[] lineNum;
    // Start is called before the first frame update
    void Start()
    {
        btn_phoneOff.onClick.AddListener(OnClickPhoneOffButton);

        objs_member = new GameObject[memberGroup.transform.childCount];
        for (int i = 0; i < memberGroup.transform.childCount; i++)
        {
            objs_member[i] = memberGroup.transform.GetChild(i).gameObject;
        }
       

        //여기서 day 알림 체크 
        DayInfo dayinfo = DatabaseManager.Instance.days[GameManager.Instance.day];
        Debug.Log(GameManager.Instance.day + " " + dayinfo.alarmEventID);
        if (dayinfo.alarmEventID != 0)
        {
            isEventActive = true;
            fileName = DatabaseManager.Instance.eventInfo[dayinfo.alarmEventID][0];
            lineNum = DatabaseManager.Instance.eventInfo[dayinfo.alarmEventID][1].Split(new char[] { '-' });
            
            text_notice.text= DatabaseManager.Instance.eventInfo[dayinfo.alarmEventID][3];

        }
        //친구 호감도별 쪽지 체크
    }
    void OnClickPhoneOffButton()
    {
        animator.SetTrigger("ActiveFalse");
        backPanel.SetActive(false);
    }
    public void ShowPhonePanel()
    {
        animator.SetTrigger("ActiveTrue");
        backPanel.SetActive(true);
        if (isEventActive)
        {
            DialogueManager.Instance.Canvas.SetActive(true);
            DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.
                GetComponent<InteractionEvent>().GetDialogue(fileName, int.Parse(lineNum[0]), int.Parse(lineNum[1])));
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
