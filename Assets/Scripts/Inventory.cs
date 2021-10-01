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
    public Image miniImg;
    public Button btn_setCurrentClothes;
    public Animator miniAnimator;

    private Slot[] slots;
    private Slot selectedSlot;


    // Start is called before the first frame update
    void Start()
    {
        //currentSelectedType = ItemType.Normal;
        btn_inventoryDown.onClick.AddListener(DownInventory);

        btnObj_use.GetComponent<Button>().onClick.AddListener(OnClickUseButton);
        if (btn_categories.Length >= 3)
        {
            btn_categories[0].onClick.AddListener(delegate { ChangeInventoryType(0); });
            btn_categories[1].onClick.AddListener(delegate { ChangeInventoryType(1); });
            btn_categories[2].onClick.AddListener(delegate { ChangeInventoryType(2); });
        }
        if (btn_setCurrentClothes != null)
        {
            btn_setCurrentClothes.onClick.AddListener(SetMiniCurrnetClothes);
        }
       
        
        slots = invenPanel.GetComponentsInChildren<Slot>();

        if (furnitures.Length >= 3)
        {
            List<Item> list = new List<Item>(GameManager.Instance.inven[ItemType.Furniture].Values);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].active == true)
                {
                    /*//추후삭제
                    int[] arr = new int[11];
                    GameManager.Instance.AddEnergy(list[i].effect[0]);
                    GameManager.Instance.AddIntimacy(list[i].effect[1]);

                    for (int j = 0; j < 11; j++)
                        arr[j] = list[i].effect[j + 2];
                    GameManager.Instance.AddAttribute(arr);
                    */
                    //활성화된 가구 보이게 설정
                    furnitures[list[i].id / 100].sprite = Resources.Load<Sprite>(list[i].itemImage);

                }

            }
        }
        else
        {
            miniAnimator.SetInteger("clothesID", GameManager.Instance.clothesId);

        }


        ClearDetailTab();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickUseButton()
    {
        switch (selectedSlot.item.type)
        {
            case ItemType.Normal:
                UseItem();
                break;
            case ItemType.Furniture:
                ChangeFurniture();
                break;
            case ItemType.Clothes:
                ChangeClothes();
                break;
        }
    }
    private void SetMiniCurrnetClothes()
    {
        for(int i=0; i< slots.Length; i++)
        {
            if (slots[i].item != null && slots[i].item.active)
            {
                Debug.Log("Inventory::SetMiniCurrnetClothes() slots[i].item.active ==true");
                slots[i].ItemClick();
                return;
            }
        }
        miniImg.sprite = Resources.Load<Sprite>("Image/미니0");
        ClearDetailTab();
    }
    private void ChangeClothes()
    {
        Debug.Log("Inventory::ChangeClothes in");
        int itemid = selectedSlot.item.id;
        
        if (GameManager.Instance.inven[ItemType.Clothes][itemid].active == false)
        {
            GameManager.Instance.inven[ItemType.Clothes][itemid].active = true;
            GameManager.Instance.AddPlusAttribute(GameManager.Instance.AllAttrsToAttrs(GameManager.Instance.inven[ItemType.Clothes][itemid].effect), 1);

            if (GameManager.Instance.clothesId != 0)
            {
                GameManager.Instance.inven[ItemType.Clothes][GameManager.Instance.clothesId].active = false;
                GameManager.Instance.AddPlusAttribute(GameManager.Instance.AllAttrsToAttrs(GameManager.Instance.inven[ItemType.Clothes][GameManager.Instance.clothesId].effect), -1);
            }
            

            GameManager.Instance.clothesId = itemid;

            //애니메이션으로 바꿔주어야함.
            miniAnimator.SetInteger("clothesID", GameManager.Instance.clothesId);
            UpdateItemDetails(selectedSlot);
        }
        else
        {
            Debug.Log("Inventory::ChangeClothes   선택한 아이템 active true일때 click");
            GameManager.Instance.inven[ItemType.Clothes][itemid].active = false;
            GameManager.Instance.AddPlusAttribute(GameManager.Instance.AllAttrsToAttrs(GameManager.Instance.inven[ItemType.Clothes][itemid].effect), -1);
            GameManager.Instance.clothesId = 0;
            miniAnimator.SetInteger("clothesID", GameManager.Instance.clothesId);
            text_selectedItemUseButton.text = "착용";
            miniImg.sprite = Resources.Load<Sprite>("Image/미니0");
        }
        
    }
    private void UseItem()
    {
        if (selectedSlot.itemCount > 0)
        {
            GameManager.Instance.AddAllAttributes(selectedSlot.item.effect);

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
        if (GameManager.Instance.inven[ItemType.Furniture][itemid].active==false)
        {         
            GameManager.Instance.inven[ItemType.Furniture][itemid].active = true;
            GameManager.Instance.AddPlusAttribute(GameManager.Instance.AllAttrsToAttrs(GameManager.Instance.inven[ItemType.Furniture][itemid].effect), 1);
            for (int i = 0; i < 4; i++)//한 가구당 종류 4가지
            {
                if (itemid % 100 != i)
                {
                    if (GameManager.Instance.inven[ItemType.Furniture].ContainsKey(itemid - (itemid % 100) + i))
                    {
                        GameManager.Instance.inven[ItemType.Furniture][itemid - (itemid % 100) + i].active = false;
                        GameManager.Instance.AddPlusAttribute(GameManager.Instance.AllAttrsToAttrs(GameManager.Instance.inven[ItemType.Furniture][itemid - (itemid % 100) + i].effect), -1);
                    }
                }
            }
            Debug.Log(itemid / 100);
            furnitures[itemid / 100].sprite = Resources.Load<Sprite>(GameManager.Instance.inven[ItemType.Furniture][itemid].itemImage);
            UpdateItemDetails(selectedSlot);
        }
        
    }

    public void UpdateItemDetails(Slot slot)
    {

        Item item = slot.item;
        selectedSlot = slot;

        if (item != null)
        {
            Debug.Log("Inventory::UpdateItemDetails() (item != null)");
            if (item.type == ItemType.Book)
            {
                btnObj_use.SetActive(false);
            }
            else
            {
                btnObj_use.SetActive(true);
                if (item.type == ItemType.Normal)
                {
                    text_selectedItemUseButton.text = "사용";                    
                }
                else if(item.type==ItemType.Furniture)
                {
                    text_selectedItemUseButton.text = GameManager.Instance.inven[currentSelectedType][item.id].active ? "사용중" : "사용";
                }
                else
                {
                    text_selectedItemUseButton.text = GameManager.Instance.inven[currentSelectedType][item.id].active ? "착용해제" : "착용";
                    Debug.Log("Inventory::UpdateItemDetails 미니 이미지 변경 ");
                    miniImg.sprite = Resources.Load<Sprite>("Image/미니"+item.id);

                }
            }    
            text_selectedItemName.text = item.name;
            text_selectedItemInfo.text = item.content;


            text_selectedItemEffect.text = DatabaseManager.Instance.GetItemEffectString(item);
        }
        else
        {
            ClearDetailTab();
        }
        
      
    }
    private void ClearDetailTab()
    {
        ResetOutLine();
        Debug.Log("Inventory::ClearDetailTab in");
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
        inventotryBackPanel.SetActive(inventoryActivated);
        animator.SetTrigger("ActiveTrue");
    }
    public void DownInventory()
    {
        ClearDetailTab();
        inventoryActivated = false;
        inventotryBackPanel.SetActive(inventoryActivated);
        animator.SetTrigger("ActiveFalse");
    }
    public void ResetOutLine()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].SetOutLine(false);
    }
    public void UpdateInventory()
    {


        ResetOutLine();
        Debug.Log("Inventory::UpdateInventory currentSelectedType:" + currentSelectedType.ToString());
        ClearDetailTab();
        if (btn_categories.Length < 3)
        {
            currentSelectedType = ItemType.Clothes;
            SetMiniCurrnetClothes();
        }
        List<Item> itemList =new List<Item>(GameManager.Instance.inven[currentSelectedType].Values);
        for (int i=0; i<slots.Length; i++)
        {
            if (itemList.Count>i)
            {
                Debug.Log(itemList[i].name);
                slots[i].AddItem(itemList[i],GameManager.Instance.inven[currentSelectedType][itemList[i].id].count);
            }
            else
            {
                slots[i].ClearSlot();
            }
              
        }
        

    }

}
