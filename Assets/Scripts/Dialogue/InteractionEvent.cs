using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractionEvent : MonoBehaviour
{
    [SerializeField] 
    public DialogueEvent dialogue;


    public Dialogue[] GetDialogue(int eventId)
    {//데이터베이스매니저에 저장된 대사 스크립트를 꺼내와야함 (몇번째줄부터 몇번째 줄까지)

        if (!GameManager.Instance.clearEventList.Contains(eventId))//이미 클리어된 이벤트가 아니면(처음보는 이벤트)
        {
            GameManager.Instance.clearEventList.Add(eventId);

            string fileName = DatabaseManager.Instance.eventInfo[eventId].fileName;
            string[] lineNum = DatabaseManager.Instance.eventInfo[eventId].line2line.Split(new char[] { '-' });
            dialogue.dialogues = DatabaseManager.Instance.GetDialogue(fileName, int.Parse(lineNum[0]), int.Parse(lineNum[1]));
        }
        else//이미 클리어되었다면 안 나와야 함
        {
            dialogue.dialogues = null;
        }
            

        return dialogue.dialogues;
    }
    
}
