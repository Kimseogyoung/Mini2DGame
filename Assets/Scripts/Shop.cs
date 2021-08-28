﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public int[][] storeItems=new int[4][]; //0 편의점, 1 옷가게, 2책방, 3 가구점
    public Button[] btn_store;
    public GameObject btn_buy;
    public GameObject btn_shopOut;
    public GameObject slotGroup;
    public GameObject backPanel;
    public GameObject shoppingBackGround;

    public Animator animator;

    public TextMeshProUGUI text_context;
    public TextMeshProUGUI text_shopName;

    public Image shopImage;

    private ShopSlot[] shopSlots;
    private int selectedItemId;

    // Start is called before the first frame update
    void Start()
    {
        
        shopSlots=slotGroup.GetComponentsInChildren<ShopSlot>();
        btn_shopOut.GetComponent<Button>().onClick.AddListener(OnClickShopOut);
        btn_buy.GetComponent<Button>().onClick.AddListener(OnClickBuyButton);

    }
    void OnClickBuyButton()
    {
        Debug.Log("buy");
        Item _item = DatabaseManager.Instance.ItemsDic[selectedItemId];
        if (GameManager.Instance.AddMoney(-_item.price))
        {
            text_context.text = "결제되었습니다.";
            GameManager.Instance.inven[_item.type == ItemType.Available ? "Normal" : _item.type.ToString()].Add(_item.id, _item);

        }
        else
        {
            text_context.text = "가지고 계신 돈이 부족합니다..";
        }
    }
    public void FinishShopping()
    {
        shoppingBackGround.SetActive(false);
        ResetOutLine();
        
    }
    public void ResetOutLine()
    {
        for (int i = 0; i < shopSlots.Length; i++)
            shopSlots[i].SetOutLine(false);
    }
    public void ShowItemDetail(Item _item)
    {
        
        btn_buy.SetActive(_item != null);
        if (_item != null)
        {
            selectedItemId = _item.id;
            string effectStr = DatabaseManager.Instance.GetItemEffectString(_item);
            text_context.text = "그건 " + _item.name + " 입니다.\n" + _item.price + "원 입니다.\n"+effectStr;
        }
        else
        {
            selectedItemId = 0;
            text_context.text = "찾으시는 거 있으시면 말씀해주세요.";
        }
        

    }
    void OnClickShopOut()
    {
        animator.SetTrigger("ActiveFalse");
        backPanel.SetActive(false);
    }
    public void InitShopping()
    {
        shoppingBackGround.SetActive(true);
        if(storeItems[0]==null)
            SetShopItems();
        btn_store[0].onClick.AddListener(delegate { OnClickStoreButton(0); });
        btn_store[1].onClick.AddListener(delegate { OnClickStoreButton(1); });
        btn_store[2].onClick.AddListener(delegate { OnClickStoreButton(2); });
        btn_store[3].onClick.AddListener(delegate { OnClickStoreButton(3); });
    }
    public void OnClickStoreButton(int num)
    {
        backPanel.SetActive(true);
        btn_buy.SetActive(false);
        switch (num)
        {
            case 0:
                text_context.text = "어서오세요 편의점입니다.";
                text_shopName.text = "짱 편의점";
                break;
            case 1:
                text_context.text = "어서오세요. 짱 미니샵 입니다.";
                text_shopName.text = "짱 미니 샵";
                break;
            case 2:
                text_context.text = "어서오세요. 미니책방 입니다.";
                text_shopName.text = "좋은 책방";
                break;
            case 3:
                text_context.text = "어서오세요. 편한가구점 입니다.";
                text_shopName.text = "편한 가구점";
                break;
        }
        
        for (int i=0; i<shopSlots.Length; i++)
        {
            if (i >= storeItems[num].Length)
            {
                shopSlots[i].SetItem();
            }
            else
            {
                Debug.Log(storeItems[num][i]);
                shopSlots[i].SetItem(DatabaseManager.Instance.ItemsDic[storeItems[num][i]]);
            }
            
        }
        animator.SetTrigger("ActiveTrue");
    }
    public void SetShopItems()
    {
        for(int i=0; i<4; i++)
        {
            ItemType type=(ItemType)i;
            List<int> list = new List<int>();
                
    
            for(int j=0; j < Mathf.Min(DatabaseManager.Instance.typeItemDic[type].Count,8);)
            {
                int num = Random.Range(0, DatabaseManager.Instance.typeItemDic[type].Count);
                if (!list.Contains(DatabaseManager.Instance.typeItemDic[type][num]))
                {
                    list.Add(DatabaseManager.Instance.typeItemDic[type][num]);
                    j++;
                }
            }
            storeItems[i] = list.ToArray();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
