using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameover : MonoBehaviour
{
    public GameObject go;
    private NPCMonster theNM;
    BGMManager BGM;
    public int playMusicTrack;

    void Start()
    {
        theNM = FindObjectOfType<NPCMonster>();
        BGM = FindObjectOfType<BGMManager>();
    }

    void Update()
    //{
    //}
    //void MusicStart()
    {
        if(theNM.fail)
        {
            Debug.Log("111");
            go.SetActive(true);
            Debug.Log("222");
            BGM.Play(playMusicTrack);
            Debug.Log("333");
            StartCoroutine(GameOverCoroutine());
        }
    }

    IEnumerator GameOverCoroutine()
    {   
        yield return new WaitForSeconds(3f);
        Debug.Log("444");
        Application.Quit();
    }

}
