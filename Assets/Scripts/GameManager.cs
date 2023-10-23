using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Bound[] bounds;
    private PlayerManager thePlayer;
    private CameraManager theCamera;
    private FadeManager theFade;
    private Menu theMenu;
    private DialogueManager theDM;
    private Camera cam;

    public GameObject hpbar;
    public GameObject mpbar;


    public void LoadStart()
    {
        StartCoroutine(LoadWaitCoroutine());
    }
    IEnumerator LoadWaitCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        thePlayer = FindObjectOfType<PlayerManager>();
        bounds = FindObjectsOfType<Bound>();
        theCamera = FindObjectOfType<CameraManager>();
        theFade = FindObjectOfType<FadeManager>();
        theMenu = FindObjectOfType<Menu>();
        theDM = FindObjectOfType<DialogueManager>();
        cam = FindObjectOfType<Camera>();

        theCamera.target = GameObject.Find("Player");
        theMenu.GetComponent<Canvas>().worldCamera = cam;
        theDM.GetComponent<Canvas>().worldCamera = cam;

        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;

        for(int i = 0; i < bounds.Length; i++)
        {
            if(bounds[i].boundName == thePlayer.currentMapName)
            {
                bounds[i].SetBound();
                break;
            }
        }

        hpbar.SetActive(true);
        mpbar.SetActive(true);

        theFade.FadeIn();
    }
}
