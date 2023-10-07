using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private FadeManager theFade;

    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
    }
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if(!flag && Input.GetKey(KeyCode.Z) && thePlayer.animator.GetFloat("DirY") == 1f)
        {
            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }
    */

    public IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move("player", "LEFT");
        theOrder.Move("player", "LEFT");
        theOrder.Move("player", "UP");
        yield return new WaitUntil(()=> thePlayer.queue.Count == 0);

        theFade.Flash();
        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

       theOrder.Move();
    }
}
