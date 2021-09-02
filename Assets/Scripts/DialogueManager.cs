using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class DialogueManager : Singleton<DialogueManager>
{
    [HideInInspector]
    public GameObject Canvas;


    public GameObject dialogUI;
    public GameObject charactorPanel;
    public GameObject backGroundPanel;
    public GameObject choicePanel;


    public TextMeshProUGUI text_name;
    public TextMeshProUGUI text_context;



    private Image charactorImage;
    private Image backGroundImage;

    private string prevBackSpriteName = "";
    private string prevSpriteName="";
    private int prevLineCount;
    private int lineCount;//대화 진행상황 카운트
    private int sentenceCount = 0; //문장 카운트



    Dialogue[] dialogues;
    bool isDialogue= false; //대화중
    bool isNext = false; //키입력 대기상태
    bool isMonolog = false; //독백상태일때
    bool isQuestion = false;//선택지 입력 대기 상태일 때

    //public Animator animSprite;

    public Button touchObject;
    public Button Skip;


    private Button btn_choice1;
    private Button btn_choice2;
    private TextMeshProUGUI text_choice1;
    private TextMeshProUGUI text_choice2;
    private void Start()
    {
        charactorImage = charactorPanel.GetComponent<Image>();
        backGroundImage = backGroundPanel.GetComponent<Image>();

        prevSpriteName = "";
        prevBackSpriteName = "";
        prevLineCount = -1;
        lineCount = 0;
        text_name.text = "";
        text_context.text = "";
        touchObject.onClick.AddListener(ShowNextDialogue);

        Canvas = gameObject.transform.GetChild(0).gameObject;
        GameObject obj_choice1 = choicePanel.transform.GetChild(0).gameObject;
        GameObject obj_choice2 = choicePanel.transform.GetChild(1).gameObject;

        btn_choice1 = obj_choice1.GetComponent<Button>();
        btn_choice2 = obj_choice2.GetComponent<Button>();
        
        btn_choice1.GetComponent<Button>().onClick.AddListener(delegate { OnClickedChoicePanel(0); });
        btn_choice2.GetComponent<Button>().onClick.AddListener(delegate { OnClickedChoicePanel(1); });

        text_choice1 = obj_choice1.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text_choice2 = obj_choice2.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        choicePanel.SetActive(false);


        //추후삭제
        Skip.onClick.AddListener(ExitDialogue);
    }


   
    void OnClickedChoicePanel(int i)
    {
        int nextId = dialogues[lineCount].questions[sentenceCount].GetNextid(i);
        Debug.Log(dialogues[lineCount].questions[sentenceCount].GetEffect());
        isNext = true;
        lineCount = nextId;
        sentenceCount = 0;
        choicePanel.SetActive(false);
        ShowNextDialogue();
        
    }

    void ShowChoicePanels()
    {
        choicePanel.SetActive(true);

        Debug.Log(lineCount+" "+sentenceCount);
        text_choice1.text 
            = dialogues[dialogues[lineCount].questions[sentenceCount].GetNextid(0)].sentences[0];
        text_choice2.text
            = dialogues[dialogues[lineCount].questions[sentenceCount].GetNextid(1)].sentences[0];


    }
    void ShowNextDialogue()
    {
        if (isNext && isDialogue)
        {
            StopAllCoroutines();

            isNext = false;
            text_context.text = "";

            if (isQuestion)//선택지후에 showNextDialogue를 호출했다면
            {         
                isQuestion = false;
                StartCoroutine(StartDialogueCoroutine());
                Debug.Log("next scene0");
                return;
            }

           
            
            if (dialogues[lineCount].questions[sentenceCount].IsJump())
            {
               
                lineCount = dialogues[lineCount].questions[sentenceCount].GetNextid();
                sentenceCount = 0;
            }
            else
            {
                sentenceCount++;
            }

            if (sentenceCount < dialogues[lineCount].sentences.Length)
            {           
                   
                    StartCoroutine(StartDialogueCoroutine());

                    Debug.Log("next scene1");               
            }
            else
            {
                sentenceCount = 0;
                if (++lineCount >= dialogues.Length)
                {
            
                    ExitDialogue();
                    Debug.Log("next scene2");
                }
                else
                {
                    
                    StartCoroutine(StartDialogueCoroutine());

                    Debug.Log("next scene3");
                }
               
            }

            
        }
       
    }
    public void ShowDialogue(Dialogue[] p_dialogues)
    {

        if (p_dialogues != null)
        {

            Canvas.SetActive(true);
            isDialogue = true;
            dialogUI.SetActive(true);
            charactorPanel.SetActive(true);

            //choicePanel.SetActive(false);

            dialogues = p_dialogues;
            StartCoroutine(StartDialogueCoroutine());
        }
        else
        {
            
            Debug.Log("p_dialogues is null or Already Cleared");
        }
       
    }
    public void ExitDialogue()
    {
       

        isDialogue = false;
        isNext = false;
        isMonolog = false;
        isQuestion = false;

        dialogUI.SetActive(false);
        charactorPanel.SetActive(false);

        prevBackSpriteName = "";
        prevSpriteName = "";
        prevLineCount = -1;
        lineCount = 0;
        sentenceCount = 0;
        text_context.text = "";
        text_name.text = "";

        Canvas.SetActive(false);
        //Camera.main.cullingMask = LayerMask.GetMask(new string[] { "MainUI", "RoomObject" });
    }

    IEnumerator StartDialogueCoroutine()
    {
        

        
        if(prevLineCount!= lineCount)
        {   //id가 달라질 때

            prevLineCount = lineCount;           
            text_name.text = dialogues[lineCount].name;//화자 이름 바꾸기

            isMonolog = dialogues[lineCount].name.Equals("");
            charactorPanel.SetActive(!isMonolog);


        }


        if (prevSpriteName != dialogues[lineCount].sprites[sentenceCount])
        {   //이미지가 달라질 때

            prevSpriteName = dialogues[lineCount].sprites[sentenceCount];            
            charactorImage.sprite = Resources.Load<Sprite>("Image/dialogImg/" 
                    + dialogues[lineCount].sprites[sentenceCount]);

        }

        if (dialogues[lineCount].backSprites[sentenceCount] != "")
        {
            if (prevBackSpriteName != dialogues[lineCount].backSprites[sentenceCount])
            {   //배경이미지가 달라질 때 
                prevBackSpriteName = dialogues[lineCount].backSprites[sentenceCount];
                backGroundImage.sprite = Resources.Load<Sprite>("Image/"
                        + dialogues[lineCount].backSprites[sentenceCount]);
            }
        }
        

        

        string t_ReplaceText = dialogues[lineCount].sentences[sentenceCount];
        t_ReplaceText = t_ReplaceText.Replace("'", ",");
        


        for(int i=0; i < t_ReplaceText.Length; i++)//0번째 문장의 길이만큼 ..뭐를 반복(한글자씩 추가하려고)
        {

            string t_letter= t_ReplaceText[i].ToString();
            if (t_ReplaceText[i] == 'ⓠ') continue;

            if (isMonolog) t_letter = "<color=#686868>" + t_letter + "</color>";
            else t_letter = "<color=#000000>" + t_letter + "</color>";

            text_context.text += t_letter;
            yield return new WaitForSeconds(0.02f);
        }

        if (t_ReplaceText.Length > 0)
        {
            if (t_ReplaceText[0] == 'ⓠ')
            {
                isQuestion = true;
                isNext = false;
                yield return new WaitForSeconds(0.1f);
                ShowChoicePanels();
            }
            else
            {
                isNext = true;
            }
            
        }
        else
        {
            isNext = true;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
