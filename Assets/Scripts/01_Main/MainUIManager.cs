using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    public ScheduleManager scheduleManager;
    public Inventory inventory;
    public Phone phone;

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
    public TextMeshProUGUI text_MainContext;
    public TextMeshProUGUI[] text_results;
    public TextMeshProUGUI text_dialogContext;
    public Button btn_dialogueNext;

    
    public Button btn_uiClean;
    public Button btn_uiOn;
    public Button btn_makeSchedule;
    public Button btn_inventory;
    public Button btn_phone;

    public Button miniNoteObject;
    public GameObject miniNote;
    public GameObject miniNoteIntimacy;
    public GameObject leftPage;
    
    public Button btn_miniNoteDel;


    public Animator animatorDiary;
    public void Start()
    {
        DialogueManager.Instance.SetDontDestroyed();
        DatabaseManager.Instance.SetDontDestroyed();
        GameManager.Instance.SetDontDestroyed();

        CheckState();
        
        UpdateUI();
        
        miniNoteObject.onClick.AddListener(ShowMiniNote);
        btn_makeSchedule.onClick.AddListener(OnClickMakeScheduleButton);
        btn_uiOn.onClick.AddListener(OnClickUIOnButton);
        btn_uiClean.onClick.AddListener(OnClickUICleanButton);
        btn_miniNoteDel.onClick.AddListener(OnClickMiniNoteDel);
        btn_dialogueNext.onClick.AddListener(OnClickNextDialogueButton);
        btn_inventory.onClick.AddListener(inventory.UpInventory);
        btn_phone.onClick.AddListener(phone.ShowPhonePanel);
        //string[] layer = new string[] { "MainUI","RoomObject" };
        //Camera.main.cullingMask = LayerMask.GetMask(layer);
        //Camera.main.cullingMask = LayerMask.GetMask(layer);
    }
    void OnClickMiniNoteDel()
    {
        animatorDiary.SetTrigger("ActiveFalse");
        miniNote.SetActive(false);
    }
    void ShowMiniNote()
    {
        RectTransform rectInti= miniNoteIntimacy.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        rectInti.sizeDelta = new Vector2( 180 * ((float)GameManager.Instance.intimacy / 100), rectInti.sizeDelta.y);

        TextMeshProUGUI IntiText = miniNoteIntimacy.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        IntiText.text = GameManager.Instance.intimacy.ToString();

        for (int i=0; i<11; i++)
        {
            GameObject obj = leftPage.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject;
            RectTransform rectTranAttr = obj.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            rectTranAttr.sizeDelta = new Vector2( rectTranAttr.sizeDelta.x, 55 * ((float)GameManager.Instance.egg.attributeValues[i] / 500));

            TextMeshProUGUI objValueText = obj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            objValueText.text = GameManager.Instance.egg.attributeValues[i].ToString();

            TextMeshProUGUI objNameText = leftPage.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            if (i >= 8 && !GameManager.Instance.egg.isOpenhidden[i % 8])
            {
                objNameText.text = "???";              
            }
            else
            {
                objNameText.text = DatabaseManager.Instance.attributeNames[i];
            }
        }
        animatorDiary.SetTrigger("ActiveTrue");
        miniNote.SetActive(true);
    }
    void CheckState()
    {
        UpdateUI();
        DayInfo dayinfo = DatabaseManager.Instance.days[GameManager.Instance.day];
        switch (GameManager.Instance.state)
        {

            case State.None:
                break;
            case State.Start:
                if (dayinfo.dayStartEventID != 0)
                {             
                    DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.
                        GetComponent<InteractionEvent>().GetDialogue(dayinfo.dayStartEventID));
                }
                GameManager.Instance.state = State.None;
                break;

            case State.Finish:
                if (dayinfo.dayFinishEventID != 0)
                {
                    Debug.Log("finish State");
                    DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.
                        GetComponent<InteractionEvent>().GetDialogue(dayinfo.dayFinishEventID));
                }
                ShowResult();
                break;
        }
        if (GameManager.Instance.state != State.Finish)
        {
            phone.CheckNewAlarm();
        }
        
    }


    private int currentDialogueIndex=0;
    private string[] dialogueContexts;
    private string[] resultStateContexts;
    void ShowMainDialogue()
    {
        bool isFinish = false;
        if (currentDialogueIndex >= dialogueContexts.Length)
        {
            isFinish = true;
            currentDialogueIndex = 0;
        }

        switch (GameManager.Instance.state)
        {
            case State.Start:
                if (isFinish)
                {

                }
                break;
            case State.Finish:
                if (isFinish)
                {
                    GameManager.Instance.day++;
                    GameManager.Instance.state = State.Start;
                    resultPanel.SetActive(false);
                    underDialoguePanel.SetActive(false);
                    CheckState();
                    //나중에 화면전환 검정색 애니메이션 추가요망
                    //저장도 여기서
                }
                
                //이때 연필 슥슥 효과음
                text_results[currentDialogueIndex].text = resultStateContexts[currentDialogueIndex];
                text_dialogContext.text = dialogueContexts[currentDialogueIndex];
                break;
        }
    }
    void OnClickNextDialogueButton()
    {
        currentDialogueIndex++;
        ShowMainDialogue();

    }
    void ShowResult()
    {
        for(int i=0; i < 3; i++)
        {
            text_results[i].text = "";
        }
        List<string> dialogueContextList = new List<string>();
        List<string> resultStateContextList = new List<string>();

        //에너지
        if (GameManager.Instance.energy >= 70)
        {
            dialogueContextList.Add("오늘 컨디션되게 좋았지!");
            resultStateContextList.Add("좋음");
        }
        else if (GameManager.Instance.energy >= 30)
        {
            dialogueContextList.Add("살짝 피곤했던 것 같기도하고.. 많이 무리하는 건 힘들 것 같아.");
            resultStateContextList.Add("괜찮음");
        }
        else{
            dialogueContextList.Add("......완전 피곤해");
            resultStateContextList.Add("나쁨");
        }

        //유대감
        if (GameManager.Instance.intimacy >= 70)
        {
            dialogueContextList.Add(GameManager.Instance.egg.name+"랑은 잘 지내고 있어");
            resultStateContextList.Add("좋음");
        }
        else if (GameManager.Instance.intimacy >= 30)
        {
            dialogueContextList.Add(GameManager.Instance.egg.name + "에게 신경을 좀 써야할 것 같아");
            resultStateContextList.Add("보통");
        }
        else
        {
            dialogueContextList.Add(GameManager.Instance.egg.name + "랑은 솔직히 좀 어색하네..");
            resultStateContextList.Add("나쁨");
        }

        //특성변화
        int changeCount=0;
        int BigChangeCount = 0;
        string attrStr = "";
        for(int i=0; i < GameManager.Instance.changedAttrs.Length; i++)
        {
            if (GameManager.Instance.changedAttrs[i] != 0)
            {
                if (changeCount % 3 == 2)
                    attrStr += "\n";
                if (i != 0)
                    attrStr += ", ";
                attrStr += DatabaseManager.Instance.attributeNames[i] + " " + GameManager.Instance.changedAttrs[i];
                changeCount++;
                if (GameManager.Instance.changedAttrs[i] >= 20)
                    BigChangeCount++;
            }
            
        }
        
        if (changeCount == 0)
        {
            attrStr += "변화 없음";
            dialogueContextList.Add("그리고 오늘은 특성변화가 하나도 없네.\n좀 더 신경을 써 주어야 할 것 같다");
        }
        else if (changeCount <= 2)
        {
            if (BigChangeCount == 0)
            {
                dialogueContextList.Add("그리고 오늘 특성변화는 미미했다..\n없는 것 보단 나을 수도?");
            }
            else 
            {
                dialogueContextList.Add("그리고.. 꽤 변동이 있는 특성이 있었네~");
            }

        }
        else if (changeCount <= 4)
        {
            if (BigChangeCount==0)
            {
                dialogueContextList.Add("그리고 미니에게는 적당히 특성변화가 있었다.");
            }
            else if(BigChangeCount<=2)
            {
                dialogueContextList.Add("그리고 오늘은 변화가 큰 특성도 있고, \n적당히 변경된 특성도 있고.. 밸런스가 좋네 ");
            }
            else
            {
                dialogueContextList.Add("그리고 오늘은 크게 변한 특성이 많았다!!\n알차게 보낸 하루였나봐?");
            }
        }
        else
        {
            dialogueContextList.Add("그리고 오늘은 전체적으로 특성에 영향이 좀 있었나보다. ");
        }

        resultStateContextList.Add(attrStr);

        dialogueContexts = dialogueContextList.ToArray();
        resultStateContexts = resultStateContextList.ToArray();

        resultPanel.SetActive(true);
        underDialoguePanel.SetActive(true);
        ShowMainDialogue();
       
    }
    void OnClickMakeScheduleButton()
    {
        if (phone.isEventActive == false)
        {
            scheduleManager.InitScheduleSelect();
        }
        else
        {
            scheduleManager.PlayAlarmAnim("읽지않은 알림이 있습니다.");
        }
        
        
    }

    void OnClickUIOnButton()
    {
        roomUI.SetActive(true);
    }
    void OnClickUICleanButton()
    {
        roomUI.SetActive(false);
    }
    void Update()
    {
        UpdateUI();
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //DialogueManager.Instance.ShowDialogue(dialogue);
            //Camera.main.cullingMask = LayerMask.GetMask(new string[] { "EventUI" });
           
            DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.GetComponent<InteractionEvent>().GetDialogue(1));

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
