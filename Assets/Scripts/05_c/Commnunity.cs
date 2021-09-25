using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class Commnunity : MonoBehaviour
{
    public TalkManager talkManager;


    public GameObject battlePanel;
    public GameObject battleSelectPanel;
    public GameObject dialoguePanel;
    public GameObject choicePanel;
    public GameObject heartBack;
    public TextMeshProUGUI text_context;
    public Image portraitImg;

    public Button btn_yes;
    public Button btn_no;

    private Animator animator;

    private Button btn_nextDialogue;

    private int talkIndex=0;
    private bool isAction = false;
    private bool isQusetion = false;
    private GameObject scanObject;

    private int heart=5;
    private Image[] heartObjs;
    private GameObject lastActionObj;
    private int currentTalkId;
    private int[] changedAttributes;

    void Start()
    {
        animator = GetComponent<Animator>();
        changedAttributes = new int[11];
        btn_nextDialogue = dialoguePanel.GetComponent<Button>();
        btn_nextDialogue.onClick.AddListener(delegate { Action(scanObject); });

        btn_no.onClick.AddListener(delegate { isQusetion = false; Action(scanObject); });
        btn_yes.onClick.AddListener(ShowNpcComponent);
        heartObjs = heartBack.GetComponentsInChildren<Image>();
    }
    public void AfterBattle(bool isWin,int typeIdx=0, int typelevel=0)
    {
        
        heart += lastActionObj.GetComponent<ObjectData>().activityPoint;
        for (int i = 4; i >= heart; i--)
        {
            heartObjs[i].color = new Color(1, 1, 1, 0);
        }

        if (isWin)
        {
            Action(lastActionObj,2000 + typelevel+1);
            
            GameManager.Instance.commuBattleLevels[typeIdx] = typelevel;
        }
        else
        {
            Action(lastActionObj, 2000 + 4);

        }

        dialoguePanel.SetActive(isAction);
    }
    void ShowNpcComponent()
    {//각 npc들의 기능 실행(scanObject에 따라서)


        isQusetion = false;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Action(scanObject);

        //여기서 scaobj사용
        if (objData.id == 2000)
        {
            if (heart+objData.activityPoint<0)
            {
                Action(lastActionObj, 2000 + 5);
            }
            else
            {
                battlePanel.SetActive(true);
                battleSelectPanel.SetActive(true);
            }
            

        }
        else if (objData.id == 1000) {
            animator.SetTrigger("finish");
        }
        
    }
    void FinishThisScene()
    {
        Debug.Log("Community::FinishThisScene in");
        GameManager.Instance.state = State.Finish;
        GameManager.Instance.changedAttrs = changedAttributes;
        SceneManager.LoadScene("Scenes/01_Main");
    }
    public void Action(GameObject _scanObject,int sp_id=0)
    {
        if (!isQusetion)
        {
            scanObject = _scanObject;
            if (scanObject != null)
            {
                ObjectData objData = scanObject.GetComponent<ObjectData>();
                Debug.Log("this is " + scanObject + " id" + objData.id);

                if (sp_id != 0)
                {
                    talk(sp_id, objData.isNpc);
                }
                else
                    talk(objData.id, objData.isNpc);

                dialoguePanel.SetActive(isAction);
            }
        }
        
    }
    void talk(int id, bool isNpc)
    {

        if (talkIndex == 0)
            currentTalkId = id;
        //set Talk
        string talkData = talkManager.GetTalk(currentTalkId, talkIndex);//(id+ questTalkIndex, talkIndex);
        
        //End Talk
        if (talkData == null)
        {
            currentTalkId = 0;
            lastActionObj = scanObject;
            scanObject = null;
            isAction = false;
            isQusetion = false;
            talkIndex = 0;
            choicePanel.SetActive(false);
            return;
        }

        if (isNpc)
        {
            if (talkData[0] == 'ⓕ')
            {
                talkData=talkData.Substring(1);
                choicePanel.SetActive(true);
                isQusetion = true;
            }
            

            text_context.text = talkData.Split(':')[0];

            portraitImg.sprite = talkManager.GetPortrait((id/1000)*1000, int.Parse(talkData.Split(':')[1])); //int.Parse는 int로 변환
            portraitImg.color = new Color(1, 1, 1, 1);//맨뒤 값이 투명도


            

        }
        else
        {
            text_context.text = talkData;
            portraitImg.color = new Color(1, 1, 1, 0);
        }
        isAction = true;
        talkIndex++;
    }
    // Update is called once per frame
    void Update()
    {

      

    }

}
