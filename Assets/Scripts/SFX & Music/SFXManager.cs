using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(SFXLibrary))]
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public SFXLibrary SFXLibrary;

    private List<EventInstance> activeSFX = new();

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

    void Update()
    {
        CleanupFinishedSFX();
    }

    /** <summary>
     * Plays a SFX that can be stopped later. Useful for long sfx.
     * </summary>
     * <param name="sfx">The SFX to play.</param>
     * <returns>The EventInstance of the played SFX.</returns>
     */
    public EventInstance PlaySFX(SFX sfx)
    {
        EventInstance inst = RuntimeManager.CreateInstance(sfx.reference);
        inst.start();
        inst.release();
        activeSFX.Add(inst);
        return inst;
    }

    public void StopAllRunningSFX(FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.IMMEDIATE)
    {
        foreach (var sfx in activeSFX)
        {
            sfx.stop(stopMode);
        }
        activeSFX.Clear();
    }

    private void CleanupFinishedSFX()
    {
        for (int i = activeSFX.Count - 1; i >= 0; i--)
        {
            EventInstance inst = activeSFX[i];

            inst.getPlaybackState(out PLAYBACK_STATE state);

            if (state == PLAYBACK_STATE.STOPPED || state == PLAYBACK_STATE.STOPPING) activeSFX.Remove(inst);
        }
    }
}
