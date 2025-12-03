using System.Collections.Generic;
using System.IO;
using FMODUnity;
using UnityEngine;

public class MusicLibrary : MonoBehaviour
{
    [SerializeField] public List<Music> MusicList;

    public Music GetMusicByName(string name)
    {
        foreach (Music music in MusicList)
        {
            if(music.Name.Equals(name)) return music;
        }

        return null;
    }

    public Music GetMusicByReference(EventReference reference)
    {
        foreach (Music music in MusicList)
        {
            if(music.reference.Equals(reference)) return music;
        }

        return null;
    }
}

[System.Serializable] public class Music
{
    public string Name => GetName();
    public EventReference reference;

    public string GetName()
    {
        RuntimeManager.GetEventDescription(reference).getPath(out string path);
        return Path.GetFileName(path);
    }
}