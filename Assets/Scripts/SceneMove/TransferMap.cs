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

    private PlayerManager thePlayer;
    private FadeManager theFade;
    private OrderManager theOrder;


    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!door)
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
                if(Input.GetKeyDown(KeyCode.Z))
                {
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
}
