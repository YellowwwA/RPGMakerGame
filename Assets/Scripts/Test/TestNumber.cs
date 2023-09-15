using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNumber : MonoBehaviour
{
	private OrderManager theOrder;
	private NumberSystem theNumber;

	public bool flag;
	public int correctNumber;

	void Start()
	{
		theOrder = FindObjectOfType<OrderManager>();
		theNumber = FindObjectOfType<NumberSystem>();
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
		theNumber.ShowNumber(correctNumber);
		yield return new WaitUntil(() => !theNumber.activated);
		theOrder.Move();
	}
}
