using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{

    public string transferMapName;

    public Animator anim_1;
    public Animator anim_2;

    public int door_count;//문 개수

    [Tooltip("UP, DOWN, LEFT, RIGHT")]
    public string direction; //캐릭터가 바라보고 있는 방향
    private Vector2 vector; //getfloat("dirX")
    
    [Tooltip("문이 있으면 : true, 문이 없으면  : false")]
    public bool door; //문이 있는지 없는지
    public bool Lock;
    public bool classroom;
    public bool finalExit;

    private PlayerManager thePlayer;
    private FadeManager theFade;
    private OrderManager theOrder;
    private Event1 theEvent;
    private NumberSystem theNumSystem;
    private Inventory theInven;
    private ChoiceManager theChoice;
    private TestChoice theTestChoice;

    private bool KeyOpen = false;
    private bool NumOpen = false;
    private bool classKey = false;
    private bool choiceOpen = false;
    

    //private int keyNum;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theInven = FindObjectOfType<Inventory>();
        theEvent = FindObjectOfType<Event1>();
        theNumSystem = FindObjectOfType<NumberSystem>();
        theChoice = FindObjectOfType<ChoiceManager>();
        theTestChoice = FindObjectOfType<TestChoice>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((!door) && (!Lock) && (!classroom) && (!finalExit))
        {
            if(collision.gameObject.name == "Player")
            {
                StartCoroutine(TransferCoroutine());
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(door)
        {
            if(collision.gameObject.name == "Player")
            {
                KeyCheck();
                if(Input.GetKeyDown(KeyCode.Z)&& KeyOpen )
                {
                    KeyOpen = false;
                    vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));
                    switch(direction)
                    {
                        case "UP":
                            if(vector.y == 1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "DOWN":
                            if(vector.y == -1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "RIGHT":
                            if(vector.x == 1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "LEFT":
                            if(vector.x == -1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        default:
                            StartCoroutine(TransferCoroutine());
                            break;
                    }
                }
            }            
        }
        else if(Lock)
        {
            //PassWordCheck();
            if(collision.gameObject.name == "Player")
            {
                PassWordCheck();
                if(NumOpen )    // Input.GetKeyDown(KeyCode.Z)&&
                {
                    StartCoroutine(TransferCoroutine());
                }
            }
        }
        else if(classroom)
        {
            if(collision.gameObject.name == "Player")
            {
                ClassKeyCheck();
                if(Input.GetKeyDown(KeyCode.Z)&& classKey )    // Input.GetKeyDown(KeyCode.Z)&&
                {
                    StartCoroutine(TransferCoroutine());
                }
            }
        }
        else if(finalExit)
        {
            if(collision.gameObject.name == "Player")
            {
                //Debug.Log("22222finalExit에 진입");
                FinalChoiceCheck();
                if( choiceOpen )    // Input.GetKeyDown(KeyCode.Z)&&
                {
                    Debug.Log("3333z키 누름");
                    StartCoroutine(TransferCoroutine());
                }
            }
        }
    }

    IEnumerator TransferCoroutine()
    {
            theOrder.PreLoadCharacter();
            theOrder.NotMove();
            theFade.FadeOut();
            if(door)
            {
                anim_1.SetBool("Open", true);
                if(door_count == 2)
                    anim_2.SetBool("Open", true);                    
            }
            yield return new WaitForSeconds(0.5f);

            theOrder.SetTransparent("player");
            if(door)
            {
                anim_1.SetBool("Open", false);
                if(door_count == 2)
                    anim_2.SetBool("Open", false);                    
            }
            yield return new WaitForSeconds(0.5f);
            theOrder.SetUnTransparent("player");
            thePlayer.currentMapName = transferMapName;
            SceneManager.LoadScene(transferMapName);
            theFade.FadeIn();
            //yield return new WaitForSeconds(0.5f);
            theOrder.Move();
    }
    private void KeyCheck()
    {
        for(int j = 0; j<theInven.inventoryItemList.Count; j++)  //소지품에 아이템이 있는지 검색
        {
            if(theInven.inventoryItemList[j].itemID == 40002)  //소지품에 키 아이템이 있다
            {
                    Debug.Log("키획득");
                    KeyOpen = true;
                    StartCoroutine(theEvent.EventCoroutine());
                    theInven.inventoryItemList.RemoveAt(j);
                    //keyNum = j;
            }
        }
    }
    private void ClassKeyCheck()
    {
        for(int j = 0; j<theInven.inventoryItemList.Count; j++)  //소지품에 아이템이 있는지 검색
        {
            if(theInven.inventoryItemList[j].itemID == 40003)  //소지품에 키 아이템이 있다
            {
                    Debug.Log("교실키획득");
                    classKey = true;
                    //StartCoroutine(theEvent.EventCoroutine());
                    theInven.inventoryItemList.RemoveAt(j);
                    //keyNum = j;
            }
        }
    }    
    public void PassWordCheck()
    {
        
        if(theNumSystem.GetResult())
        {
            //StartCoroutine(TransferCoroutine());

            NumOpen = true;
        }
    }
    public void FinalChoiceCheck()
    {
            //Debug.Log(theChoice.GetResult());
        if(theTestChoice.cA)
        {
            //Debug.Log(theChoice.result);
            //Debug.Log(theChoice.correct);
            //Debug.Log(theChoice.GetResult());
            choiceOpen = true;
        }
    }
}
