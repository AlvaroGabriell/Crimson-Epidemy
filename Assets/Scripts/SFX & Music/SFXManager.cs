using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SFXLibrary))]
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public SFXLibrary SFXLibrary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SFXLibrary = GetComponent<SFXLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlaySFX(SFX sfx)
    {
        RuntimeManager.PlayOneShot(sfx.reference);
    }
}
