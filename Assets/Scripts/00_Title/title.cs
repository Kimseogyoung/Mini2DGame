using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class title : MonoBehaviour
{
    public GameObject nameObject;
    public TMP_InputField nameInput;
    public Button btn_start;
    // Start is called before the first frame update
    private void Awake()
    {
        DialogueManager.Instance.SetDontDestroyed();
        DatabaseManager.Instance.SetDontDestroyed();
        GameManager.Instance.SetDontDestroyed();

    }
    void Start()
    {

        btn_start.onClick.AddListener(OnClickStartButton);
        //SoundManager.Instance.PlayBGM();
        GameManager.Instance.SetDontDestroyed();
    }

    void OnClickStartButton()
    {
        Debug.Log(nameInput.text.Length + " " + nameInput.text);
        if (nameInput.text.Length <= 5 && nameInput.text.Length > 0)
        {
            for (int i = 0; i < nameInput.text.Length; i++)
            {
                if(nameInput.text[i]==' ' )
                {
                    return;
                }
            }
            GameManager.Instance.playerName = nameInput.text;
            SceneManager.LoadScene("Scenes/01_Main");
            GameManager.Instance.state = State.Start;//추후 바꾸기 저장데이터로 
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount>0 || Input.GetMouseButtonDown(0))
        {
            if (!GameManager.Instance.playerName.Equals(""))
            {
                SceneManager.LoadScene("Scenes/01_Main");
            }
            else
            {
                nameObject.SetActive(true);
            }

            //SceneManager.LoadScene("Scenes/01_Main");
        }
        
    }
}
