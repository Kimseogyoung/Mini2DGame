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
    public Button btn_inventoryDown;
    public Button[] btn_categories;
    public Animator animator;

    public TextMeshProUGUI text_selectedItemName;
    public TextMeshProUGUI text_selectedItemInfo;
    public TextMeshProUGUI text_selectedItemEffect;

    private Slot[] slots;
    
    // Start is called before the first frame update
    void Start()
    {
        
        btn_inventoryDown.onClick.AddListener(DownInventory);


        btn_categories[0].onClick.AddListener(delegate { ChangeInventoryType(0); });
        btn_categories[1].onClick.AddListener(delegate { ChangeInventoryType(1); });
        btn_categories[2].onClick.AddListener(delegate { ChangeInventoryType(2); });
        
        slots = invenPanel.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateItemDetails(Item item)
    {

        text_selectedItemName.text = item.name;
        text_selectedItemInfo.text= item.content;

        string str = "";
        for(int i=0; i<13; i++)
        {
            if (i < 2)
            {
                
            }
            else
            {
                str = DatabaseManager.Instance.attributeNames[i - 2] + " " + item.effect[i];
            }
            
        }
        text_selectedItemEffect.text = str;
      
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

        GameManager.Instance.inventory["Normal"][0] = 1;
        Debug.Log(currentSelectedType.ToString());
        int[] itemList =GameManager.Instance.inventory[currentSelectedType.ToString()];
        for (int i=0; i<12; i++)
        {
            if (itemList[i] != 0)
            {
                Debug.Log(DatabaseManager.Instance.ItemsDic[itemList[0]].name);
                slots[i].AddItem(DatabaseManager.Instance.ItemsDic[itemList[i]]);
            }
            else
            {
                slots[i].ClearSlot();
            }
            
                
        }

    }

}
