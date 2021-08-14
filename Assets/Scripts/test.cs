using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //DialogueManager.Instance.ShowDialogue(dialogue);
            Camera.main.cullingMask = LayerMask.GetMask(new string[] { "EventUI" });
            DialogueManager.Instance.ShowDialogue(gameObject.GetComponent<InteractionEvent>().GetDialogue("당근마켓", 2, 19));

        }
        if (Input.GetKeyDown(KeyCode.Z))
        {

            
        }

    }
}
