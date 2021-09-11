using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScheduleType { normal, shoping, book, alba, friend }
[System.Serializable]
public class Schedule 
{
    public int id;
    public string name;
    public ScheduleType type;
    public int money;
    public int energy;
    public int intimacy;

    public int[] attributeValues;
    public string[] dialogueContext;
    public string[] photo;

    public int friend=-1;

  
}
