using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battle : MonoBehaviour
{
    public GameObject selectPanel;
    public Button[] btns_select;
    public Button btn_attack;

    public GameObject playerScoreBar;
    public GameObject middleScoreBar;
    public TextMeshProUGUI text_playerScore;
    public TextMeshProUGUI text_playerName;
    public TextMeshProUGUI text_playerMiniName;

    public TextMeshProUGUI text_enemyScore;
    public TextMeshProUGUI text_enemyName;
    public TextMeshProUGUI text_enemyMiniName;

    private int currentPlayerScore=50;
    private int battleType;//0 1 2 3
    // Start is called before the first frame update
    void Start()
    {
        btns_select[0].onClick.AddListener(delegate { OnClickSelectButtons(0); });
        btns_select[1].onClick.AddListener(delegate { OnClickSelectButtons(1); });
        btns_select[2].onClick.AddListener(delegate { OnClickSelectButtons(2); });
        btns_select[3].onClick.AddListener(delegate { OnClickSelectButtons(3); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FinishGame()
    {

    }
    void StartGame()
    {
        //SetGameStart에서 실행한 애니메이션이 끝날때 이벤트로 실행되는 함수.
        //score확인
        if(currentPlayerScore<=0 || currentPlayerScore >= 100)
        {
            //finish
            FinishGame();
        }
        btn_attack.onClick.AddListener(OnClickAttackButton);
    }
    void OnClickAttackButton()
    {
        //애니메이션 실행, 실행끝나면 start Game함수 이벤트호출
    }

    void SetGameStart()
    {
        //난이도 설정후
        //시작 애니메이션 실행
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
