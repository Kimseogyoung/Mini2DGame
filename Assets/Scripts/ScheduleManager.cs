using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class ScheduleManager : MonoBehaviour
{

    
    public GameObject schedulePanel;
    public GameObject scheduleDialogueObject;
    //selecting part
    public int currentPage;
    public GameObject scheduleLayout;
    public GameObject scheduleBackPanel;//시간표 
    public GameObject btn_prevPage;
    public GameObject btn_nextPage;
    public GameObject btn_start;
    public TextMeshProUGUI text_moneymessage;

    public GameObject selectSlotGroup;
    public TextMeshProUGUI text_selectSlotGroup;


    //
    private int[] selectedSchedule;//선택된 스케쥴의 idx저장
    private int maxPage = 4;

    private Button[] btn_schedule;
    private TextMeshProUGUI[] text_scheduleName;
    private TextMeshProUGUI[] text_scheduleAttr;

    private GameObject[] selectedScheduleObject;
    private TextMeshProUGUI[] text_selectedScheduleName;
    private Button[] btn_deleteSchedule;

    private ScheduleSlot[] scheduleSlots;
    

    //scheduling part

    private void Awake()
    {
       
    }
    void Start()
    {
        

        btn_schedule = new Button[4];
        text_scheduleName = new TextMeshProUGUI[4];
        text_scheduleAttr = new TextMeshProUGUI[4];

        selectedScheduleObject = new GameObject[3];
        text_selectedScheduleName = new TextMeshProUGUI[3];
        btn_deleteSchedule = new Button[3];

        currentPage = 0;
        for(int i=0; i < 4; i++)
        {
            
            GameObject obj = scheduleLayout.transform.GetChild(i).gameObject;
            btn_schedule[i] = obj.GetComponent<Button>();
            text_scheduleName[i] = obj.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            text_scheduleAttr[i] = obj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        }
        btn_schedule[0].onClick.AddListener(delegate { OnClickScheduleSelectButton(0); });
        btn_schedule[1].onClick.AddListener(delegate { OnClickScheduleSelectButton(1); });
        btn_schedule[2].onClick.AddListener(delegate { OnClickScheduleSelectButton(2); });
        btn_schedule[3].onClick.AddListener(delegate { OnClickScheduleSelectButton(3); });

        GameObject schedulePanel = scheduleBackPanel.transform.GetChild(0).gameObject;

        for(int i=0; i<3; i++)
        {
            selectedScheduleObject[i] = schedulePanel.transform.GetChild(i).gameObject;
            selectedScheduleObject[i].SetActive(false);

            text_selectedScheduleName[i] = selectedScheduleObject[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            text_selectedScheduleName[i].text = "";

            btn_deleteSchedule[i]= selectedScheduleObject[i].transform.GetChild(1).gameObject.GetComponent<Button>();
            
        }
        btn_deleteSchedule[0].onClick.AddListener(delegate { OnClickDeleteScheduleButton(0); });
        btn_deleteSchedule[1].onClick.AddListener(delegate { OnClickDeleteScheduleButton(1); });
        btn_deleteSchedule[2].onClick.AddListener(delegate { OnClickDeleteScheduleButton(2); });
        btn_prevPage.GetComponent<Button>().onClick.AddListener(OnClickPrevButton);
        btn_nextPage.GetComponent<Button>().onClick.AddListener(OnClickNextButton);
        btn_start.GetComponent<Button>().onClick.AddListener(OnClickStartButton);
        
        scheduleSlots=selectSlotGroup.GetComponentsInChildren<ScheduleSlot>();

    }


    private int check;
    public void ShowSelectBookOrFriend(int id=0)
    {
        if (id != 0)
        {
            selectedSchedule[check] = id;
            Debug.Log(check + "id"+selectedSchedule[check]);
        }
        if (++check == 3)
        {
            GameManager.Instance.selectedSchedule = selectedSchedule;
            schedulePanel.SetActive(false);
            scheduleDialogueObject.SetActive(false);
            SceneManager.LoadScene("Scenes/02_Schedule");
            return;
        }

        
        if (selectedSchedule[check] == 5 || selectedSchedule[check] == 15)
        {
            List<Item> list=new List<Item>();
            if (selectedSchedule[check] == 5)
            {
                text_selectSlotGroup.text = (check + 1) + "번 일정 책 고르기";
                list= new List<Item>(GameManager.Instance.inven["Book"].Values);
            }
            else
            {
                text_selectSlotGroup.text = (check + 1) + "번 일정 연락할 친구 선택하기";
                //list= new List<Item>(GameManager.Instance.inven["Book"].Values);
            }
            
                selectSlotGroup.SetActive(true);
                
                if (selectedSchedule[check] == 5)
                {
                    
                    for (int i = 0; i < scheduleSlots.Length; i++)
                    {
                        if (list.Count > i)
                        {
                            scheduleSlots[i].SetSlot(list[i]);
                        }
                        else
                        {
                            scheduleSlots[i].SetSlot(null);
                        }

                    }
                }
        }
        else
        {
            selectSlotGroup.SetActive(false);
            ShowSelectBookOrFriend();
         }


    }

    public void OnClickStartButton()
    {
        check = -1;
        ShowSelectBookOrFriend();
        
        
    }


    public void InitScheduleSelect()
    {
        UpdateScheduleUI();
        schedulePanel.SetActive(true);
        scheduleDialogueObject.SetActive(true);

        scheduleBackPanel.SetActive(true);
        scheduleLayout.SetActive(true);

        btn_start.SetActive(false);

        currentPage = 0;
        text_moneymessage.text = "";
        selectedSchedule = new int[] { -1, -1, -1 };
        for(int i=0; i<3; i++)
        {
            selectedScheduleObject[i].SetActive(false);
            text_selectedScheduleName[i].text = "";
        }
    }
    public void CheckSchedule()
    {
        int count = 0;
        int currentMoney = GameManager.Instance.money;
        bool bookCheck=true;
        for (int i = 0; i < 3; i++)
        {
            if (selectedSchedule[i] != -1)
            {
                count++;

                currentMoney += DatabaseManager.Instance.scheduleDic[selectedSchedule[i]].money;
                if (currentMoney < 0)
                {
                    text_moneymessage.text = "스케쥴을 수행하기 위한 돈이 부족합니다.";
                    break;
                }
                if (selectedSchedule[i] == 5 && GameManager.Instance.inven["Book"].Count <= 0)
                {
                    bookCheck = false;
                    text_moneymessage.text = "읽을 수 있는 책이 없습니다.";
                    break;
                }
                text_moneymessage.text = "";
                

            }
        }
        

        if (count >= 3 && currentMoney>=0 && bookCheck) btn_start.SetActive(true);
        else
        {
            btn_start.SetActive(false);
            if (count == 0)
            {
                text_moneymessage.text = "";
            }
        }
    }
    public void OnClickDeleteScheduleButton(int btnnum)
    {
        selectedSchedule[btnnum] = -1;
        text_selectedScheduleName[btnnum].text = "";
        selectedScheduleObject[btnnum].SetActive(false);
        CheckSchedule();
    }
    public void OnClickScheduleSelectButton(int btnnum)
    {
        
        for(int i=0; i < 3; i++)
        {
            if (selectedSchedule[i] == -1)
            {
                selectedSchedule[i] = btnnum + (4 * currentPage);
                Debug.Log("this is " + btnnum + " " + currentPage + " " + selectedSchedule[i]);
                text_selectedScheduleName[i].text = DatabaseManager.Instance.scheduleDic[selectedSchedule[i]].name;
                selectedScheduleObject[i].SetActive(true);
                break;
            }
        }
        CheckSchedule();


        
    }
    public void UpdateScheduleUI()
    {
        if (currentPage <= 0)
        {
            btn_prevPage.SetActive(false);
        }
        else if(currentPage >= maxPage-1)
        {
            btn_nextPage.SetActive(false);
        }
        else
        {
            btn_nextPage.SetActive(true);
            btn_prevPage.SetActive(true);
        }
        

        for (int i = 0; i < 4; i++)
        {
            List<Schedule> schedules = new List<Schedule>(DatabaseManager.Instance.scheduleDic.Values);
            if (schedules[i + 4 * currentPage] != null)
            {
                text_scheduleName[i].text = schedules[i + 4 * currentPage].name;
                string str = "에너지 " + schedules[i + 4 * currentPage].energy;
                if (schedules[i + 4 * currentPage].money != 0)
                {
                    str += "돈 " + schedules[i + 4 * currentPage].money;
                }
                else if (schedules[i + 4 * currentPage].intimacy != 0)
                {
                    str += "유대감 " + schedules[i + 4 * currentPage].intimacy;
                }
                for (int j = 0; j < 11; j++)
                {
                    if (schedules[i + 4 * currentPage].attributeValues[j] != 0)
                    {
                        str += DatabaseManager.Instance.attributeNames[j] + " " + schedules[i + 4 * currentPage].attributeValues[j];
                    }
                }
                text_scheduleAttr[i].text = str;
            }
           
        }
    }
    public void OnClickPrevButton()
    {
        currentPage--;    
        UpdateScheduleUI();
    }
    public void OnClickNextButton()
    {
        currentPage++;
        UpdateScheduleUI();
    }
    

}
