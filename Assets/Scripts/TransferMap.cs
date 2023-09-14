using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{

    public string transferMapName;

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
        if(collision.gameObject.name == "Player")
        {
            StartCoroutine(TransferCoroutine());
        }
    }

    IEnumerator TransferCoroutine()
    {
            theOrder.NotMove();
            theFade.FadeOut();

            yield return new WaitForSeconds(1f);
            thePlayer.currentMapName = transferMapName;
            SceneManager.LoadScene(transferMapName);
            theFade.FadeIn();
            //yield return new WaitForSeconds(0.5f);
            theOrder.Move();
    }
}
