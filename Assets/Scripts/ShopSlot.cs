using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public int slotNum;
    public Shop shop;
    public Image itemImage;
    Item item;
    
    private Outline outline;

    public int GetItemId()
    {
        if (item == null)
            return 0;
        else 
            return item.id;
    }
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
        shop.ShowItemDetail(slotNum);
        SetOutLine(true);
    }
    public void SoldOut(bool value)
    {
        //흑백 조절
        if (value)
        {
            itemImage.color = new Color(0.5f,0.5f,0.5f);

        }
        else
        {
            itemImage.color = new Color(1,1,1);
        }
        
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
    public void SetIndex(int num)
    {
        slotNum = num;
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
