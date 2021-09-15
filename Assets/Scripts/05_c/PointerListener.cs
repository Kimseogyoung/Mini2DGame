using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerListener : MonoBehaviour, IPointerExitHandler, IPointerUpHandler ,IPointerDownHandler
{
    public bool _pressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
    }
      public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        _pressed = false;
    }
  
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}



