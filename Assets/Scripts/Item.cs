using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { Available, Normal, Book, Furniture };

public class Item 
{
    

    public string name;
    public string content;
    public ItemType type;
    public int[] effect=new int[13];

    public string itemImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
