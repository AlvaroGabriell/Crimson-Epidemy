using System.IO;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(MusicLibrary))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    
    public MusicLibrary musicLibrary;
    public EventInstance currentMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            musicLibrary = GetComponent<MusicLibrary>();
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
        SetMusicParameter("enemyCount", GameController.Instance.enemyCount);
    }

    public string GetCurrentEventName()
    {
        currentMusic.getDescription(out var description);
        description.getPath(out string path);
        return Path.GetFileName(path);
    }

    public void PlayMusic(Music music)
    {
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }

        currentMusic = RuntimeManager.CreateInstance(music.reference);

        currentMusic.start();
    }

    public void StopMusic()
    {
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }
    }

    public void SetMusicParameter(string name, float value)
    {
        if (currentMusic.isValid())
        {
            currentMusic.setParameterByName(name, value);
        }
    }
}
