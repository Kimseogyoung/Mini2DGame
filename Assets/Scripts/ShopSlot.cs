using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Shop shop;
    public Image itemImage;
    Item item;
    
    private Outline outline;

    public void SetOutLine(bool value)
    {
        outline.enabled = value;
    }

    private void SetColor(float alpha)
    {
        //투명조절
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }
    public void ItemClick()
    {
        shop.ResetOutLine();
        shop.ShowItemDetail(item);
        SetOutLine(true);
    }
    public void SetItem(Item _item=null)
    {
        
        item = _item;
        if (item != null)
        {
            itemImage.sprite = Resources.Load<Sprite>(item.itemImage);
            SetColor(255);
        }
        else
        {
            itemImage.sprite = null;
            SetColor(0);
        }
           

    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ItemClick);
        item = null;
        SetColor(0);
        outline = gameObject.GetComponent<Outline>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
