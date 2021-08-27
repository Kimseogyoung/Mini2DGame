using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public bool inventoryActivated = false;
    public ItemType currentSelectedType = ItemType.Normal;

    public GameObject inventotryBackPanel;
    public GameObject invenPanel;

    public GameObject btnObj_use;
    public Button btn_inventoryDown;
    public Button[] btn_categories;
    public Animator animator;

    public TextMeshProUGUI text_selectedItemName;
    public TextMeshProUGUI text_selectedItemInfo;
    public TextMeshProUGUI text_selectedItemEffect;
    public TextMeshProUGUI text_selectedItemUseButton;

    public SpriteRenderer[] furnitures;

    private Slot[] slots;
    private Slot selectedSlot;
    // Start is called before the first frame update
    void Start()
    {
        
        btn_inventoryDown.onClick.AddListener(DownInventory);

        btnObj_use.GetComponent<Button>().onClick.AddListener(OnClickUseButton);
        btn_categories[0].onClick.AddListener(delegate { ChangeInventoryType(0); });
        btn_categories[1].onClick.AddListener(delegate { ChangeInventoryType(1); });
        btn_categories[2].onClick.AddListener(delegate { ChangeInventoryType(2); });
        
        slots = invenPanel.GetComponentsInChildren<Slot>();
        
        //추후삭제
        List<Item> list = new List<Item>(GameManager.Instance.inven["Furniture"].Values);
        for (int i=0; i<list.Count; i++)
        {
            if (list[i].active == true)
            {

                int[] arr = new int[11];
                GameManager.Instance.AddEnergy(list[i].effect[0]);
                GameManager.Instance.AddIntimacy(list[i].effect[1]);

                for (int j = 0; j < 11; j++)
                    arr[j] = list[i].effect[j + 2];
                GameManager.Instance.AddAttribute(arr);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickUseButton()
    {
        switch (selectedSlot.item.type)
        {
            case ItemType.Available:
                UseItem();
                break;
            case ItemType.Normal:
                SetActiveNormalItem(!GameManager.Instance.inven["Normal"][selectedSlot.item.id].active);
                break;
            case ItemType.Furniture:
                ChangeFurniture();
                break;
        }
    }
    private void SetActiveNormalItem(bool value)
    {
        
        GameManager.Instance.inven["Normal"][selectedSlot.item.id].active = value;
        int plus = (value == true ? 1 : -1);
        int[] arr = new int[11];
        GameManager.Instance.AddEnergy(plus * selectedSlot.item.effect[0]);
        GameManager.Instance.AddIntimacy(plus * selectedSlot.item.effect[1]);
        for (int i = 0; i < 11; i++)
                arr[i] = plus * selectedSlot.item.effect[i + 2];
        GameManager.Instance.AddAttribute(arr);

        UpdateItemDetails(selectedSlot);
        
        

    }
    private void UseItem()
    {
        if (selectedSlot.itemCount > 0)
        {
;           int[] arr = new int[11];
            GameManager.Instance.AddEnergy(selectedSlot.item.effect[0]);
            GameManager.Instance.AddIntimacy(selectedSlot.item.effect[1]);
            for (int i = 0; i < 11; i++)
                arr[i] = selectedSlot.item.effect[i + 2];
            GameManager.Instance.AddAttribute(arr);

            selectedSlot.SetSlotCount(-1);
            if (selectedSlot.item == null)
            {
                ClearDetailTab();
            }
        }
        

    }
    private void ChangeFurniture()
    {
        int itemid = selectedSlot.item.id;
        if (GameManager.Instance.inven["Furniture"][itemid].active==false)
        {         
            GameManager.Instance.inven["Furniture"][itemid].active = true;
            for (int i = 0; i < 3; i++)
            {
                if (itemid % 100 != i)
                {
                    if (GameManager.Instance.inven["Furniture"].ContainsKey(itemid - (itemid % 100) + i))
                    {
                        GameManager.Instance.inven["Furniture"][itemid - (itemid % 100) + i].active = false;
                    }
                }
            }
            Debug.Log(itemid / 100);
            furnitures[itemid / 100].sprite = Resources.Load<Sprite>(GameManager.Instance.inven["Furniture"][itemid].itemImage);
            UpdateItemDetails(selectedSlot);
        }
        
    }

    public void UpdateItemDetails(Slot slot)
    {

        Item item = slot.item;
        selectedSlot = slot;

        if (item != null)
        {
            if(item.type == ItemType.Book)
            {
                btnObj_use.SetActive(false);
            }
            else
            {
                btnObj_use.SetActive(true);
                if (item.type != ItemType.Available)
                {
                    text_selectedItemUseButton.text = GameManager.Instance.inven[currentSelectedType.ToString()][item.id].active ? "사용중" : "사용";
                }
                else
                {
                    text_selectedItemUseButton.text = "사용";

                }
            }    
            text_selectedItemName.text = item.name;
            text_selectedItemInfo.text = item.content;

            string str = "";
            for (int i = 0; i < 13; i++)
            {
                if (i < 2)
                {

                }
                else
                {
                    if(item.effect[i]!=0)
                        str += DatabaseManager.Instance.attributeNames[i - 2] + " " + item.effect[i]+"  ";
                }

            }
            text_selectedItemEffect.text = str;
        }
        else
        {
            ClearDetailTab();
        }
        
      
    }
    private void ClearDetailTab()
    {
        selectedSlot = null;
        text_selectedItemName.text = "";
        text_selectedItemInfo.text = "";
        text_selectedItemEffect.text = "";
        btnObj_use.SetActive(false);
    }
    public void ChangeInventoryType(int type)
    {
        switch (type)
        {
            case 0:
                currentSelectedType = ItemType.Normal;              
                break;
            case 1:
                currentSelectedType = ItemType.Book;
                break;
            case 2:
                currentSelectedType = ItemType.Furniture;
                break;
        }
        for(int i=0; i < 3; i++)
        {
            btn_categories[i].gameObject.GetComponent<Image>().color = new Color(190 / 255f, 190 / 255f, 190 / 255f, 1.0f); 
            if (i == type)
            {
                btn_categories[i].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            
        }
        
        UpdateInventory();
    }
    public void UpInventory()
    {
        UpdateInventory();
        inventoryActivated = true;
        inventotryBackPanel.SetActive(true);
        animator.SetTrigger("ActiveTrue");
    }
    public void DownInventory()
    {
        inventoryActivated = false;
        inventotryBackPanel.SetActive(false);
        animator.SetTrigger("ActiveFalse");
    }
    public void UpdateInventory()
    {
        


        List<Item> itemList =new List<Item>(GameManager.Instance.inven[currentSelectedType.ToString()].Values);
        for (int i=0; i<12; i++)
        {
            if (itemList.Count>i)
            {
                Debug.Log(itemList[i].name);
                slots[i].AddItem(itemList[i],GameManager.Instance.inven[currentSelectedType.ToString()][itemList[i].id].count);
            }
            else
            {
                slots[i].ClearSlot();
            }
              
        }
        ClearDetailTab();

    }

}
