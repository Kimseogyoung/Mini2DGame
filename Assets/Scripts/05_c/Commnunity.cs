using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class Commnunity : MonoBehaviour
{
    public TalkManager talkManager;
    public Battle battle;

    public GameObject dialoguePanel;
    public GameObject choicePanel;
    public TextMeshProUGUI text_context;
    public Image portraitImg;

    public Button btn_yes;
    public Button btn_no;

    private Button btn_nextDialogue;

    private int talkIndex=0;
    private bool isAction = false;
    private bool isQusetion = false;
    private GameObject scanObject;

    // Start is called before the first frame update

    void Start()
    {
        btn_nextDialogue = dialoguePanel.GetComponent<Button>();
        btn_nextDialogue.onClick.AddListener(delegate { Action(scanObject); });

        btn_no.onClick.AddListener(delegate { isQusetion = false; Action(scanObject); });
        btn_yes.onClick.AddListener(ShowNpcComponent);
    }
    void ShowNpcComponent()
    {//각 npc들의 기능 실행(scanObject에 따라서)
        isQusetion = false;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        //여기서 scaobj사용
        if (objData.id == 1000)
        {
            battle.Init();
        }

        Action(scanObject);
    }
    public void Action(GameObject _scanObject)
    {
        if (!isQusetion)
        {
            scanObject = _scanObject;
            if (scanObject != null)
            {
                ObjectData objData = scanObject.GetComponent<ObjectData>();
                Debug.Log("this is " + scanObject + " id" + objData.id);


                talk(objData.id, objData.isNpc);
                dialoguePanel.SetActive(isAction);
            }
        }
        
    }
    void talk(int id, bool isNpc)
    {

        //set Talk
        string talkData = talkManager.GetTalk(id, talkIndex);//(id+ questTalkIndex, talkIndex);

        //End Talk
        if (talkData == null)
        {
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

            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1])); //int.Parse는 int로 변환
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
