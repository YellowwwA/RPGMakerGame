using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;
    
    private OrderManager theOrder;
    private DialogueManager theDM;

    // Start is called before the first frame update
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theDM = FindObjectOfType<DialogueManager>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            StartCoroutine(TestDialogueCoroutine());
        }
    }
    IEnumerator TestDialogueCoroutine()
    {
        theOrder.NotMove();
        theDM.ShowDialogue(dialogue);
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move();

    }
}
