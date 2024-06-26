using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChoice : MonoBehaviour
{
    [SerializeField]
    public Choice choice;
    public int correctAnswer;

    private OrderManager theOrder;
    private ChoiceManager theChoice;
    private AudioManager theAudio;
    public string cancel_sound;
    public string correct_sound;

    public bool flag;
    public bool cA = false;

    // Start is called before the first frame update
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        theAudio = FindObjectOfType<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(!flag)
        //{
        //    StartCoroutine(ACoroutine());
        //}
        if(theChoice.result + 1 == correctAnswer)
        {
            Debug.Log(theChoice.result);
            flag = true;
        }
        if(!flag)
            StartCoroutine(ACoroutine());
    }
    IEnumerator ACoroutine()
    {
        //flag = true;
        theOrder.NotMove();
        theChoice.ShowChoice(choice);
        yield return new WaitUntil(()=>!theChoice.choiceIng);
        theOrder.Move();
        if (theChoice.result + 1 == correctAnswer)
        {
            cA = true;
            theAudio.Play(correct_sound);
        }
        else
        {
            cA = false;
            theAudio.Play(cancel_sound);
        }
   
        //Debug.Log(theChoice.GetResult());
        //Debug.Log(theChoice.result);
        //Debug.Log(theChoice.correct);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
