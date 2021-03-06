using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    public Inventory inven;
    public Item item;
    public int itemCount;
    public Image itemImage;

    public TextMeshProUGUI text_count;
    public GameObject go_CountImage;

    private Outline outline;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ItemClick);
        outline = gameObject.GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetOutLine(bool value)
    {
        outline.enabled = value;
    }

    public void ItemClick()
    {
        inven.ResetOutLine();
        inven.UpdateItemDetails(this);
        SetOutLine(true);
    }
    
    private void SetColor(float alpha)
    {
        //투명조절
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;

        itemImage.sprite = Resources.Load<Sprite>(item.itemImage);
           

        if (item.type == ItemType.Normal)
        {
            go_CountImage.SetActive(true);
            text_count.text = itemCount.ToString();
        }
        else
        {
            text_count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(255);
    }

    public void SetSlotCount(int _count)
    {///해당 슬롯 아이템 갯수 업데이트
        //count만큼 증가

        itemCount += _count;
        text_count.text = itemCount.ToString();
        GameManager.Instance.inven[item.type == ItemType.Normal ? ItemType.Normal : item.type][item.id].count = itemCount;
        if (itemCount <= 0)
        {
            GameManager.Instance.inven[item.type == ItemType.Normal ? ItemType.Normal : item.type].Remove(item.id);
            ClearSlot();
        }
    }
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_count.text = "0";
        go_CountImage.SetActive(false);
    }
}
