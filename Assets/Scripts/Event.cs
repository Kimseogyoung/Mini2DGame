using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType { Only, Always}//only- 기록됨/한번만 달성   Always-회차마다 초기화
public class Event
{
    public EventType eventType;

    //public int eventId;
    public string fileName;
    public string line2line;
    public string eventName;
    public string content;
}
