using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    public ScheduleManager scheduleManager;
    public GameObject roomUI;
    public GameObject roomObjects;

    public GameObject underPanel;
    public GameObject underDialoguePanel;
    public GameObject resultPanel;

    public GameObject choicePanel;
    public GameObject diaryPanel;
    public GameObject phonePanel;
    public GameObject optionPanel;


    public GameObject energyBar;
    public TextMeshProUGUI text_day;
    public TextMeshProUGUI text_money;
    public TextMeshProUGUI text_energy;
    public TextMeshProUGUI text_EggName;
    public TextMeshProUGUI text_playerName;
    public TextMeshProUGUI text_dialogContext;

    public Button btn_uiOn;
    public Button btn_makeSchedule;
    public Button btn_dialogueNext;


    public Button miniNoteObject;
    public GameObject miniNote;
    public GameObject miniNoteIntimacy;
    public GameObject leftPage;
    
    public Button btn_miniNoteDel;
    public void Start()
    {
       

        CheckState();
        
        UpdateUI();
        
        miniNoteObject.onClick.AddListener(ShowMiniNote);
        btn_makeSchedule.onClick.AddListener(OnClickMakeScheduleButton);
        btn_uiOn.onClick.AddListener(OnClickUIOnButton);
        btn_miniNoteDel.onClick.AddListener(OnClickMiniNoteDel);
        //string[] layer = new string[] { "MainUI","RoomObject" };
        //Camera.main.cullingMask = LayerMask.GetMask(layer);
        //Camera.main.cullingMask = LayerMask.GetMask(layer);
    }
    void OnClickMiniNoteDel()
    {
        miniNote.SetActive(false);
    }
    void ShowMiniNote()
    {
        RectTransform rectInti= miniNoteIntimacy.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        rectInti.sizeDelta = new Vector2( 200 * ((float)GameManager.Instance.intimacy / 100), rectInti.sizeDelta.y);

        for (int i=0; i<11; i++)
        {
            GameObject obj = leftPage.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject;
            RectTransform rectTranAttr = obj.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            rectTranAttr.sizeDelta = new Vector2( rectTranAttr.sizeDelta.x, 60 * ((float)GameManager.Instance.egg.attributeValues[i] / 500));

            TextMeshProUGUI objValueText = obj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            objValueText.text = GameManager.Instance.egg.attributeValues[i].ToString();

            TextMeshProUGUI objNameText = leftPage.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            if (i >= 8 && !GameManager.Instance.egg.isOpenhidden[i % 8])
            {
                objNameText.text = "???";              
            }
            else
            {
                objNameText.text = GameManager.Instance.egg.attributeNames[i];
            }
        }
        
        miniNote.SetActive(true);
    }
    void CheckState()
    {
        DayInfo dayinfo = DatabaseManager.Instance.days[GameManager.Instance.day];
        switch (GameManager.Instance.state)
        {

            case State.None:
                break;
            case State.Start:
                Debug.Log("gddi"+ dayinfo.dayStartEventID+" "+GameManager.Instance.day);
                if (dayinfo.dayStartEventID != 0)
                {
                    Debug.Log("gi");
                    string fileName = DatabaseManager.Instance.eventInfo[dayinfo.dayStartEventID][0];
                    string[] lineNum = DatabaseManager.Instance.eventInfo[dayinfo.dayStartEventID][1].Split(new char[] { '-' });
                    DialogueManager.Instance.Canvas.SetActive(true);
                    DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.
                        GetComponent<InteractionEvent>().GetDialogue(fileName, int.Parse(lineNum[0]), int.Parse(lineNum[1])));
                }
                GameManager.Instance.state = State.None;
                break;

            case State.Finish:
                if (dayinfo.dayFinishEventID != 0)
                {
                    Debug.Log("gi");
                    string fileName = DatabaseManager.Instance.eventInfo[dayinfo.dayFinishEventID][0];
                    string[] lineNum = DatabaseManager.Instance.eventInfo[dayinfo.dayFinishEventID][1].Split(new char[] { '-' });
                    DialogueManager.Instance.Canvas.SetActive(true);
                    DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.
                        GetComponent<InteractionEvent>().GetDialogue(fileName, int.Parse(lineNum[0]), int.Parse(lineNum[1])));
                }
                ShowResult();
                break;
        }
    }
    void ShowResult()
    {
        resultPanel.SetActive(true);
        underDialoguePanel.SetActive(true);
        //클릭시
        GameManager.Instance.day++;
        GameManager.Instance.state = State.Start;
        resultPanel.SetActive(false);
        underDialoguePanel.SetActive(false);
        CheckState();
    }
    void OnClickMakeScheduleButton()
    {
        scheduleManager.InitScheduleSelect();
        
    }

    void OnClickUIOnButton()
    {
        roomUI.SetActive(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //DialogueManager.Instance.ShowDialogue(dialogue);
            //Camera.main.cullingMask = LayerMask.GetMask(new string[] { "EventUI" });
           
            DialogueManager.Instance.Canvas.SetActive(true);
            DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.GetComponent<InteractionEvent>().GetDialogue("당근마켓",2,19));

        }
        if (Input.GetKeyDown(KeyCode.Z))
        {

            UpdateUI();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {

            roomUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {                   
            scheduleManager.InitScheduleSelect();
        }
    }
    public void UpdateNames()
    {
        text_playerName.text = GameManager.Instance.playerName+" 님의 반려 미니";
        text_EggName.text = GameManager.Instance.egg.name;
    }
    public void UpdateTopUI()
    {
        text_day.text = GameManager.Instance.day + "일차";
        text_money.text = GameManager.Instance.money + "원";
        text_energy.text = GameManager.Instance.energy.ToString();
        RectTransform rectTranEnergy = energyBar.GetComponent<RectTransform>();
        rectTranEnergy.sizeDelta = new Vector2(140 * ((float)GameManager.Instance.energy / 100), rectTranEnergy.sizeDelta.y);
    }




    public void UpdateUI()
    {
        UpdateTopUI();
        UpdateNames();
    }


    
}
