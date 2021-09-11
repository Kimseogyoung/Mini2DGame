using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
///인스펙터창에 띄울 수 있게
public class Dialogue
{

    [Tooltip("대사 치는 캐릭터 이름")]
    public string name;

    [TextArea (1,2)]//두줄로 늘어남
    [Tooltip("대사 내용")]
    public string[] sentences;
    public string[] sprites;
    public string[] backSprites;

    public Question[] questions;
    public int friend=-1;
    public int friendshipPoint=0;
    public int money=0;
    public int[] attrs=new int[Attrs.allAttrs];


  
}


[System.Serializable]
public class Question
{
    private int[] choices = new int[2];


    public Question( int choice1, int choice2)
    {
      
        choices[0] = choice1;
        choices[1] = choice2;

    }
    public int GetNextid(int p_choice = 0)
    {
        return choices[p_choice];
    }
    public bool IsJump()
    {
        return (choices[0]!=0 && choices[1] == 0);
    }



}

[System.Serializable]
public class DialogueEvent
{
    public string name;//어떤 이벤트인지

    public Dialogue[] dialogues;
}
