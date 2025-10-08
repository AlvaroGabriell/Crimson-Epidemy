using System;
using System.Collections.Generic;
using UnityEngine;

public class SFXLibrary : MonoBehaviour
{
    [SerializeField] private SFXGroup[] sfxGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;

    void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach (SFXGroup sfxGroup in sfxGroups)
        {
            soundDictionary[sfxGroup.name] = sfxGroup.audioClips;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = soundDictionary[name];
            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }
        return null;
    }
}

[System.Serializable] public struct SFXGroup
{
    public string name;
    public List<AudioClip> audioClips;
}