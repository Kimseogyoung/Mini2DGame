using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractionEvent : MonoBehaviour
{
    [SerializeField] 
    public DialogueEvent dialogue;


    public Dialogue[] GetDialogue(string csv_FileName, int startLine, int FinishLine)
    {//데이터베이스매니저에 저장된 대사 스크립트를 꺼내와야함 (몇번째줄부터 몇번째 줄까지)
        
        dialogue.dialogues = DatabaseManager.Instance.GetDialogue(csv_FileName, startLine, FinishLine);
        return dialogue.dialogues;
    }
    
}
