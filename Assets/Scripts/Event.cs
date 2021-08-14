using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType { GlobalDay, Count, Day, Time}
public class Event
{
    public bool isClear=false;//이벤트를 완료했는지
    public EventType eventType;
    public int scheduleId;
    public int eventId;

}
