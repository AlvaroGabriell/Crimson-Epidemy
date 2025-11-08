using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("masterVolume", volume);
    }
    public void SetVolumeBGM(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("volumeBGM", volume);
    }

    public void SetVolumeSFX (float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("volumeSFX",volume);
    }
}
