using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNumber : MonoBehaviour
{
	private OrderManager theOrder;
	//private NumberSystem theNumber;
	private DialogueManager theDM; //part21

	public bool flag;
	//public int correctNumber;
	public string[] texts; //part21

	void Start()
	{
		theOrder = FindObjectOfType<OrderManager>();
		//theNumber = FindObjectOfType<NumberSystem>();
		theDM = FindObjectOfType<DialogueManager>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(!flag)
		{
			StartCoroutine(BCoroutine());
		}
	}
	IEnumerator BCoroutine()
	{
		flag = true;
		theOrder.NotMove();
		//theNumber.ShowNumber(correctNumber);
		theDM.ShowText(texts);//part21
		//yield return new WaitUntil(() => !theNumber.activated);
		yield return new WaitUntil(() => !theDM.talking);//part21
		theOrder.Move();
	}
}
