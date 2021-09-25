using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Scheduling : MonoBehaviour
{
    public Shop shop;


    public TextMeshProUGUI text_currentScheduleName;
    public TextMeshProUGUI text_currentStateContext;
    public Image schedulePhoto;
    public GameObject currentImageObject;
    public GameObject btn_nextSchedule;
    
    public GameObject energyBar;
    public GameObject intimacyBar;
    public GameObject clockStick;

    public Button btn_nextSchedule2;
    public Animator animator;

    private int[] changedAttributes;

    private int currentSchedule;//현재 진행되고있는 스케쥴
    private int[] selectedSchedule;//선택된 스케쥴의 idx저장
    private bool isIntimacyOut = false;
    // Start is called before the first frame update
    void Start()
    {
        changedAttributes = new int[11];
        GameManager.Instance.state = State.Schedule;
        currentSchedule = 0;
        btn_nextSchedule.GetComponent<Button>().onClick.AddListener(OnClickNextScheduleButton);
        btn_nextSchedule2.onClick.AddListener(OnClickNextScheduleButton);
        selectedSchedule = GameManager.Instance.selectedSchedule;

        InitScheduling();
        clockStick.transform.localEulerAngles = new Vector3(0, 0, -90);
 
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //DialogueManager.Instance.ShowDialogue(dialogue);
            //Camera.main.cullingMask = LayerMask.GetMask(new string[] { "EventUI" });
            StopAllCoroutines();
            DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.GetComponent<InteractionEvent>().GetDialogue(1));

        }
       
    }
    public void ShowScheduleResult()
    {

    }
    void CheckEvent()
    {
    

        Schedule schedule = DatabaseManager.Instance.scheduleDic[selectedSchedule[currentSchedule]];
        List<int> arr = new List<int>(DatabaseManager.Instance.eventDic.Keys);
        arr.Sort();

        //이벤트 확인하기
        if (schedule.friend != -1)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] >= (schedule.friend+2)*100)
                    break;

                if (arr[i] >= ((schedule.friend+1) * 100))
                {
                    for (; (arr[i] <= ((schedule.friend + 1) * 100) + GameManager.Instance.friendshipPoints[schedule.friend]); i++)
                    {
                        if (!GameManager.Instance.clearEventList.Contains(arr[i]))//클리어하지 않은 이벤트 일때
                        {

                            DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.
                            GetComponent<InteractionEvent>().GetDialogue(arr[i]));
                            return;
                        }

                    }

                }
            }

        }

       

        for (int i=0;i< arr.Count; i++)
        {
            if (arr[i] >= (schedule.id + 1) * 100000)
                break;

            if (arr[i] >= (schedule.id * 100000) )
            {
                for(; arr[i] <= (schedule.id * 100000) + GameManager.Instance.scheduleCount[schedule.id]; i++)
                {
                    if (!GameManager.Instance.clearEventList.Contains(arr[i]))//클리어하지 않은 이벤트 일때
                    {
                        DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.
                        GetComponent<InteractionEvent>().GetDialogue(arr[i]));
                        return;
                    }
                        
                }
                
            }
        }
       
    }
    public void OnClickNextScheduleButton()
    {

        //스케쥴카운트 ++

        if (selectedSchedule[currentSchedule] == 1)//
        {
            CheckEvent();
        }

        if (++currentSchedule <= 2)
        {
            if (selectedSchedule[currentSchedule-1] == 1)
            {
                shop.FinishShopping();
            }
            StopAllCoroutines();
            InitScheduling();
        }
        else
        {

            animator.SetTrigger("finish");
        }

    }
    void FinishThisScene()
    {
        currentSchedule = 0;
        ShowScheduleResult();
        GameManager.Instance.state = State.Finish;
        GameManager.Instance.changedAttrs = changedAttributes;
        SceneManager.LoadScene("Scenes/01_Main");
    }
    public void InitScheduling()
    {
        StartCoroutine(MoveClockStickAndEnergy());
        StartCoroutine(StartSchedulingCoroutine());
    }
    IEnumerator MoveClockStickAndEnergy()
    {

        clockStick.transform.localEulerAngles = new Vector3(0, 0, -90*(1+currentSchedule));
        RectTransform rectTranEnergy = energyBar.GetComponent<RectTransform>();
        rectTranEnergy.sizeDelta = new Vector2(400 * ((float)GameManager.Instance.energy / 100), rectTranEnergy.sizeDelta.y);

        RectTransform rectTranIntimacy = intimacyBar.GetComponent<RectTransform>();
        rectTranIntimacy.sizeDelta = new Vector2(400 * ((float)GameManager.Instance.intimacy / 100), rectTranIntimacy.sizeDelta.y);

        for (int i = 0; i < 100; i++)
        {

            //12초
            Vector3 vec = new Vector3(0, 0, -(float)90 / 100);
            clockStick.transform.Rotate(vec);

            Vector2 energy= new Vector2(Mathf.Min(400, rectTranEnergy.sizeDelta.x + 400 * ((float)DatabaseManager.Instance.scheduleDic[selectedSchedule[currentSchedule]].energy / 10000)),
                rectTranEnergy.sizeDelta.y);
            if (energy.x<= 0)
            {
                animator.SetTrigger("energyOut");
                rectTranEnergy.sizeDelta = new Vector2(0, rectTranEnergy.sizeDelta.y);
                StopAllCoroutines();
            }
            else
            {
                rectTranEnergy.sizeDelta = energy;
            }

            Vector2 intimacy = new Vector2(Mathf.Min(400, rectTranIntimacy.sizeDelta.x + 400 * ((float)DatabaseManager.Instance.scheduleDic[selectedSchedule[currentSchedule]].intimacy / 10000)),
                rectTranIntimacy.sizeDelta.y);
            if (intimacy.x <= 0 )
            {
                if (isIntimacyOut == false)
                {
                    isIntimacyOut = true;
                    animator.SetTrigger("intimacyOut");
                }

                rectTranIntimacy.sizeDelta = new Vector2(0, rectTranIntimacy.sizeDelta.y);
            }
            else
            {
                isIntimacyOut = false;
                rectTranIntimacy.sizeDelta = intimacy;
            }


            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator StartSchedulingCoroutine()
    {


        btn_nextSchedule.SetActive(false);
        Schedule schedule = DatabaseManager.Instance.scheduleDic[selectedSchedule[currentSchedule]];
        string str = "";
        switch (currentSchedule)
        {
            case 0:
                str = "아침 일정 - ";
                break;
            case 1:
                str = "낮 일정 - ";
                break;
            case 2:
                str = "저녁 일정 - ";
                break;
        }
        text_currentScheduleName.text = str + schedule.name;

        if (selectedSchedule[currentSchedule] != 1)
        {
            for (int i = 0; i < 6; i++)
            {
                if ((schedule.dialogueContext.Length > i))
                    text_currentStateContext.text = schedule.dialogueContext[i].Replace("ⓜ", GameManager.Instance.egg.name);
                else
                    text_currentStateContext.text = "...";

                if (i != 5)
                {

                    schedulePhoto.sprite = Resources.Load<Sprite>("Image/schedulePhoto/" + schedule.photo[i % 2]);
                    yield return new WaitForSeconds(2.0f);
                }
                else
                {
                    yield return new WaitForSeconds(1.0f);
                }


            }


            text_currentStateContext.text = "";

            GameManager.Instance.money += schedule.money;
            if (schedule.money > 0)
            {
                text_currentStateContext.text += schedule.money + " 원을 벌었습니다!\n";
            }
            else if (schedule.money < 0)
            {
                text_currentStateContext.text += (-schedule.money) + " 원을 사용했습니다!\n";
            }


            if (GameManager.Instance.AddEnergy(schedule.energy))
            {
                if (schedule.energy > 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    text_currentStateContext.text += "푹 쉬어서 에너지가" + schedule.energy + " 만큼 증가했습니다.\n";
                }
                else if (schedule.energy < 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    text_currentStateContext.text += "피곤하여 에너지가 " + schedule.energy + " 만큼 감소했습니다.\n";
                }
            }
            else
            {
                text_currentStateContext.text += "에너지가 바닥났습니다..\n";
            }

            GameManager.Instance.AddIntimacy(schedule.intimacy);

            if (schedule.intimacy > 0)
            {
                yield return new WaitForSeconds(0.5f);
                text_currentStateContext.text += "유대감이 " + schedule.intimacy + " 만큼 증가했습니다.\n";
            }
            else if (schedule.intimacy < 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    text_currentStateContext.text += "유대감이 " + schedule.intimacy + " 만큼 감소했습니다.\n";
                }

            bool changed = false;
            for (int j = 0; j < 11; j++)
            {
                if (schedule.attributeValues[j] != 0)
                {
                    changed = true;
                    break;
                }
            }
            yield return new WaitForSeconds(0.5f);

            string str2 = "";
            if (changed)
            {
                if (isIntimacyOut)                    {
                    for (int i = 0; i < Attrs.attrs; i++)
                    {
                        if (schedule.attributeValues[i] > 0)
                        {
                            schedule.attributeValues[i] = -schedule.attributeValues[i];
                        }
                    }
                }

                GameManager.Instance.AddAttribute(schedule.attributeValues);

                str2 = "미니의 특성이 변화했습니다!\n";

                for (int j = 0; j < Attrs.attrs; j++)
                {
                    if (schedule.attributeValues[j] != 0)
                    {
                        changedAttributes[j] += schedule.attributeValues[j];
                        str2 += DatabaseManager.Instance.attributeNames[j] + " " + schedule.attributeValues[j] + " ";
                    }
                }

            }
            else
            {
                str2 = "미니가 학습한 특성이 없습니다.\n";
            }
            yield return new WaitForSeconds(0.2f);
            text_currentStateContext.text += str2;

        }
        else
        {
            GameManager.Instance.AddEnergy(schedule.energy);
            shop.InitShopping();
        }


        btn_nextSchedule.SetActive(true);
        GameManager.Instance.scheduleCount[selectedSchedule[currentSchedule]]++;
        if (selectedSchedule[currentSchedule] != 1)//
        {
            CheckEvent();
        }
        
    }
}

