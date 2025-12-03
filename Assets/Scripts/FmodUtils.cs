using System;
using FMODUnity;
using UnityEngine;

public class FmodUtils : MonoBehaviour
{
    public static FmodUtils Instance;
    public StudioEventEmitter music;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopMusic()
    {
        music.Stop();
    }
}
