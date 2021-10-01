using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { Normal,Clothes, Book, Furniture };

public class Item 
{

    //고정
    public int id;
    public string name;
    public string content;
    public ItemType type;
    public int[] effect=new int[13];
    public string itemImage;
    public int price;

    //
    public int count=1;
    public bool active=false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
