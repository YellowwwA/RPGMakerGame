using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    static public WeatherManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private AudioManager theAudio;
    public ParticleSystem rain;
    public string rain_sound;

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void Rain()
    {
        theAudio.Play(rain_sound);
        rain.Play();
    }
    public void RainStop()
    {
        theAudio.Stop(rain_sound);
        rain.Stop();
    }
    public void RainDrop()
    {
        rain.Emit(10);
    }
}
