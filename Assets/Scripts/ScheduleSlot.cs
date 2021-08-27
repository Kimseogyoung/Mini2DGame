using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScheduleSlot : MonoBehaviour
{
    public ScheduleManager scheduleManager;
    public Image icon;
    public TextMeshProUGUI text_name;
    public TextMeshProUGUI text_effect;

    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickSlot);
    }

    public void SetSlot(Item _item)
    {
        item = _item;
        if (item != null)
        {
            icon.sprite = Resources.Load<Sprite>(item.itemImage);
            text_name.text = item.name;
            string str = "";
            for (int i = 0; i < 13; i++)
            {
                if (i < 2)
                {

                }
                else
                {
                    if (item.effect[i] != 0)
                        str += DatabaseManager.Instance.attributeNames[i - 2] + " " + item.effect[i] + "  ";
                }

            }
            text_effect.text = str;
        }
        else
        {
            text_name.text = "";
            text_effect.text = "";
        }
        
    }
    private void OnClickSlot()
    {
        scheduleManager.ShowSelectBookOrFriend(item.id);   
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
