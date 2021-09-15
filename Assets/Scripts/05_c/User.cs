using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class User : MonoBehaviour
{
    public Commnunity commnunity;
    public PointerListener[] btns_move;
    public Button btn_action;

    int speed = 500;
    bool playerMoving;
    GameObject scanObject;
    private Rigidbody2D rigid;
    enum Dir { Up,Down,Right,Left};
    Dir dir;
    Vector3 dirVec;
    // Start is called before the first frame update
    void Start()
    {
        btn_action.onClick.AddListener(delegate { commnunity.Action(scanObject); });
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMoving = false;
        FollowCamera();
        if (btns_move[0]._pressed)
        {
            MoveUp();
        }
        else if (btns_move[1]._pressed)
        {
            MoveDown();
        }
        else if (btns_move[2]._pressed)
        {
            MoveRight();
        }
        else if (btns_move[3]._pressed)
        {
            MoveLeft();
        }
       

        if (dir == Dir.Up)
            dirVec = Vector3.up;
        else if (dir == Dir.Down)
            dirVec = Vector3.down;
        else if (dir == Dir.Left)
            dirVec = Vector3.left;
        else if (dir == Dir.Right)
            dirVec = Vector3.right;

        //Ray
        Debug.DrawRay(rigid.position, dirVec * 100.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 100.0f, LayerMask.GetMask("Obj"));

        //rayHit안에있는 collider에 무언가 있다면
        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
            scanObject = null;
    }

    void FollowCamera()
    {
        Vector3 came = Camera.main.transform.position;
        Vector3 v = new Vector3(transform.position.x, came.y, came.z);
        if (v.x >= 0 && v.x <= 700)
        {
            Camera.main.transform.position = v;
        }
    }

    public void MoveUp()
    {
        dir = Dir.Up;
        playerMoving = true;
        transform.Translate(new Vector3(0f, speed * Time.deltaTime, 0f));
    }
    public void MoveDown()
    {
        dir = Dir.Down;
        playerMoving = true;
        transform.Translate(new Vector3(0f, -speed * Time.deltaTime, 0f));
    }

    public void MoveRight()
    {
        dir = Dir.Right;
        playerMoving = true;
        transform.Translate(new Vector3(speed * Time.deltaTime, 0f, 0f));
    }
    public void MoveLeft()
    {
        dir = Dir.Left;
        playerMoving = true;
        transform.Translate(new Vector3(-speed * Time.deltaTime, 0f, 0f));
        
    }
}
