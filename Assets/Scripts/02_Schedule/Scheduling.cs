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

    private int[] changedAttributes;

    private int currentSchedule;//현재 진행되고있는 스케쥴
    private int[] selectedSchedule;//선택된 스케쥴의 idx저장
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
            DialogueManager.Instance.Canvas.SetActive(true);
            DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.gameObject.GetComponent<InteractionEvent>().GetDialogue("당근마켓", 2, 19));

        }
       
    }
    public void ShowScheduleResult()
    {

    }

    public void OnClickNextScheduleButton()
    {
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
            currentSchedule = 0;
            ShowScheduleResult();
            GameManager.Instance.state = State.Finish;
            GameManager.Instance.changedAttrs = changedAttributes;
            SceneManager.LoadScene("Scenes/01_Main");

        }

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


            rectTranEnergy.sizeDelta = new Vector2(Mathf.Min(400, rectTranEnergy.sizeDelta.x + 400 * ((float)DatabaseManager.Instance.scheduleDic[selectedSchedule[currentSchedule]].energy / 10000)),
                rectTranEnergy.sizeDelta.y);
            rectTranIntimacy.sizeDelta = new Vector2(Mathf.Min(400, rectTranIntimacy.sizeDelta.x + 400 * ((float)DatabaseManager.Instance.scheduleDic[selectedSchedule[currentSchedule]].intimacy / 10000)),
                rectTranIntimacy.sizeDelta.y);


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

            if (GameManager.Instance.AddIntimacy(schedule.intimacy))
            {
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
                    GameManager.Instance.AddAttribute(schedule.attributeValues);

                    str2 = "미니의 특성이 변화했습니다!\n";

                    for (int j = 0; j < 11; j++)
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
                text_currentStateContext.text += "미니와 사이가 나빠져 미니가 아무것도 학습하지 않았습니다.\n";
            }
        }
        else
        {
            GameManager.Instance.AddEnergy(schedule.energy);
            shop.InitShopping();
        }

        

        btn_nextSchedule.SetActive(true);
    }
}
