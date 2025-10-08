using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SFXLibrary))]
public class SFXManager : MonoBehaviour
{
    private static SFXManager Instance;

    private static AudioSource audioSource;
    private static SFXLibrary sfxLibrary;
    [SerializeField] private Slider sfxSlider;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            sfxLibrary = GetComponent<SFXLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = sfxLibrary.GetRandomClip(soundName);
        if (audioClip != null) audioSource.PlayOneShot(audioClip);
    }
    public static void Play(string soundName, AudioSource substitute)
    {
        AudioClip audioClip = sfxLibrary.GetRandomClip(soundName);
        if (audioClip != null) substitute.PlayOneShot(audioClip);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
