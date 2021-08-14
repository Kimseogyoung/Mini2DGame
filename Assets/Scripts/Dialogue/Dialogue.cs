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


  
}

public enum AfterEffect { None, EnergyUp, EnergyDown, EggCleanUp, EggCleanDown };
public class Question
{
    private int[] choices = new int[2];
    private AfterEffect effect = AfterEffect.None;

    public Question( int choice1, int choice2, AfterEffect p_effect=AfterEffect.None)
    {
      
        choices[0] = choice1;
        choices[1] = choice2;
        effect = p_effect;
    }
    public int GetNextid(int p_choice = 0)
    {
        return choices[p_choice];
    }
    public bool IsJump()
    {
        return (choices[0]!=0 && choices[1] == 0);
    }
    public AfterEffect GetEffect()
    {
        return effect;
    }



}

[System.Serializable]
public class DialogueEvent
{
    public string name;//어떤 이벤트인지

    public Dialogue[] dialogues;
}
