using System;
using System.Collections.Generic;
using System.IO;
using FMODUnity;
using UnityEngine;

public class SFXLibrary : MonoBehaviour
{
    [SerializeField] public List<SFX> SFXList;

    public SFX GetSFXByName(string name)
    {
        foreach (SFX sfx in SFXList)
        {
            if(sfx.Name.Equals(name)) return sfx;
        }

        return null;
    }

    public SFX GetSFXByReference(EventReference reference)
    {
        foreach (SFX sfx in SFXList)
        {
            if(sfx.reference.Equals(reference)) return sfx;
        }

        return null;
    }
}

[Serializable] public class SFX
{
    public string Name => GetName();
    public EventReference reference;

    public string GetName()
    {
        return Path.GetFileName(reference.Path);
    }
}