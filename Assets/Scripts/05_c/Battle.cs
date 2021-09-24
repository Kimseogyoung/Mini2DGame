using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battle : MonoBehaviour
{
    public Commnunity commnunity;

    private Animator animator;
    public GameObject selectPanel;
    public Button[] btns_select;
    public GameObject attackObj;

    private Button btn_attack;

    public GameObject playerScoreBar;
    public GameObject middleScoreBar;
    public TextMeshProUGUI text_top;
    public TextMeshProUGUI text_middle;
    public TextMeshProUGUI text_playerScore;
    public TextMeshProUGUI text_playerName;
    public TextMeshProUGUI text_playerMiniName;

    public TextMeshProUGUI text_enemyScore;
    public TextMeshProUGUI text_enemyName;
    public TextMeshProUGUI text_enemyMiniName;

    private int currentPlayerScore=50;
    private int battleType;//0 1 2 3
    private int battleLevel;
    private bool isWin;
    private float userPower;
    private float comPower;
    private bool isUserTurn=false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        btns_select[0].onClick.AddListener(delegate { OnClickSelectButtons(0); });
        btns_select[1].onClick.AddListener(delegate { OnClickSelectButtons(1); });
        btns_select[2].onClick.AddListener(delegate { OnClickSelectButtons(2); });
        btns_select[3].onClick.AddListener(delegate { OnClickSelectButtons(3); });
        btn_attack = attackObj.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OffPanel()
    {
        Debug.Log("call afterBattle");
        isUserTurn = false;
        gameObject.SetActive(false);
        commnunity.AfterBattle(isWin,battleType,battleLevel);
    }
    void FinishGame()
    {
        text_middle.alignment = TextAlignmentOptions.Center;
        RectTransform rect = middleScoreBar.GetComponent<RectTransform>();
        if (rect.anchoredPosition.x <= 0)
        {
            //플레이어 패
            animator.SetBool("isUserWin", false);
            text_middle.text = "com 승리";
            isWin = false;

            
        }
        else if (rect.anchoredPosition.x >= 720)
        {

            //플레이어 승
            animator.SetBool("isUserWin", true);
            text_middle.text = "나 승리";
            isWin = true;
        }
        animator.SetBool("Finish", true);

    }


    void OnClickAttackButton()
    {
        //애니메이션 실행, 실행끝나면 start Game함수 이벤트호출
        animator.SetTrigger("userAttack");
        attackObj.SetActive(false);
    }


    IEnumerator MoveEnergybar()
    {
        Debug.Log("에너지바 이동");
        int randNum = Random.Range(-5, 6);
        float power;
        if (isUserTurn)
        {
            power = (userPower /2.0f/ 10.0f + randNum)*(720.0f/100.0f);
        }
        else
        {
            power = -(comPower /2.0f/ 10.0f + randNum)* (720.0f / 100.0f);
        }
    

        RectTransform rect = middleScoreBar.GetComponent<RectTransform>();
        Vector2 pos = rect.anchoredPosition;

        RectTransform rectPlayer= playerScoreBar.GetComponent<RectTransform>();
        Vector2 posPlayer = rectPlayer.sizeDelta;


        for (int i=0; i<100; i++)//2초
        {
            
            float posx = power/100.0f;
          
            if (rect.anchoredPosition.x+posx>=720 )
            {
                rectPlayer.sizeDelta = new Vector2(720, posPlayer.y);
                rect.anchoredPosition = new Vector2(720, pos.y);
            }
            else if(rect.anchoredPosition.x + posx <= 0)
            {
                rectPlayer.sizeDelta = new Vector2(0, posPlayer.y);
                rect.anchoredPosition = new Vector3(0,pos.y);
            }
            else
            {
                rectPlayer.sizeDelta = new Vector2(rectPlayer.sizeDelta.x+posx, posPlayer.y);
                rect.Translate(new Vector2(posx, 0));
            }

            yield return new WaitForSeconds(0.01f);
        }


        isUserTurn = !isUserTurn;
        StartGame();

    }
    //밑에 애니메이션 끝날때 참조됨(이벤트)
    void StartGame()
    {
        //SetGameStart에서 실행한 애니메이션이 끝날때 이벤트로 실행되는 함수.
        //score확인
        RectTransform rect = middleScoreBar.GetComponent<RectTransform>();
        if (rect.anchoredPosition.x <= 0 || rect.anchoredPosition.x >=720)
        {
            //finish
            text_top.text = "대결 종료!";
            Debug.Log("배틀 끝");
            
            FinishGame();
            return;
        }
        if (isUserTurn)
        {
            Debug.Log("유저턴");
            text_middle.text = "내 차례!";
            text_middle.alignment = TextAlignmentOptions.Left;
            animator.SetTrigger("userTurn");
            btn_attack.onClick.AddListener(OnClickAttackButton);
            attackObj.SetActive(true);
        }
        else
        {
            Debug.Log("comturn");
            text_middle.text = "com 차례!";
            text_middle.alignment = TextAlignmentOptions.Right;
            animator.SetTrigger("comAttack");
        }
        
    }
    void SetGameStart()
    {
        //난이도 설정후
        //난이도 하 
        comPower = 150;
        battleLevel = 1;

        //난이도 중 플레이어 300
        //comPower = 300;

        //난이도 상 플레이어 500
        //comPower = 500;


        currentPlayerScore = 50;
        

        switch (battleType)
        {
            case 0:
                userPower = GameManager.Instance.egg.attributeValues[0];
                break;
            case 1:
                userPower = (GameManager.Instance.egg.attributeValues[2]+GameManager.Instance.egg.attributeValues[6])/2;
                break;
            case 2:
                userPower = (GameManager.Instance.egg.attributeValues[1]+GameManager.Instance.egg.attributeValues[3])/2;
                break;
            case 3:
                userPower = GameManager.Instance.egg.attributeValues[4];
                break;
        }
        //시작 애니메이션 실행
        text_top.text = "대결 시작!";
        animator.SetTrigger("Start");
    }
    void OnClickSelectButtons(int i)
    {
        battleType = i;
        selectPanel.SetActive(false);
        SetGameStart();
    }
    public void Init()
    {
        gameObject.SetActive(true);//추후 애니메이션 처리
        selectPanel.SetActive(true);


    }
}
