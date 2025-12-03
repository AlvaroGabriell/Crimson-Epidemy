using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    [Header("VCAs")]
    [SerializeField] private string masterVCAPath = "vca:/Master_VCA";
    [SerializeField] private string musicVCAPath = "vca:/Music_VCA", sfxVCAPath = "vca:/SFX_VCA";

    private VCA masterVCA, musicVCA, sfxVCA;

    void Awake()
    {
        masterVCA = RuntimeManager.GetVCA(masterVCAPath);
        musicVCA = RuntimeManager.GetVCA(musicVCAPath);
        sfxVCA = RuntimeManager.GetVCA(sfxVCAPath);
    }

    void Start()
    {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetsfxVolume(sfxVolume);
    }

    public void SetMasterVolume(float volume)
    {
        masterVCA.setVolume(volume);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        musicVCA.setVolume(volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetsfxVolume(float volume)
    {
        sfxVCA.setVolume(volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }


}
