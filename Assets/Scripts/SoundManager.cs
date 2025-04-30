using System;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioEffect
{
    Reward,
    Resource,
    Engine,
    Timer
}
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource resSound;
    [SerializeField] private AudioSource shipSound;
    [SerializeField] private AudioSource timerSound;

    private bool vibroEnabled = true;

    [Inject] private readonly GameEvents gameEvents;

    #region MONO
    private void Awake()
    {
        ambientMusic.loop = true;
    }

    private void OnEnable()
    {
        gameEvents.VibroEvent += PlayVibro;
        gameEvents.SoundEvent += PlaySound;
        gameEvents.VibroSettingsEvent += TurnVibration;
        gameEvents.SoundSettingsEvent += TurnSound;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("_SoundOn"))
        {
            PlayerPrefs.SetInt("_SoundOn", 1);
            PlayerPrefs.SetInt("_VibroOn", 1);
        }
        TurnSound(PlayerPrefs.GetInt("_SoundOn") == 1);
        TurnVibration(PlayerPrefs.GetInt("_VibroOn") == 1);
    }

    private void OnDisable()
    {
        gameEvents.VibroEvent -= PlayVibro;
        gameEvents.SoundEvent -= PlaySound;
        gameEvents.VibroSettingsEvent -= TurnVibration;
        gameEvents.SoundSettingsEvent -= TurnSound;
    }
    #endregion


    public void PlayVibro()
    {
        if (vibroEnabled)
        {
            Handheld.Vibrate();
        }
    }

    public void PlaySound(AudioEffect id)
    {
        AudioSource source;
        source = id switch
        {
            AudioEffect.Reward => winSound,
            AudioEffect.Resource => resSound,
            AudioEffect.Engine => shipSound,
            AudioEffect.Timer => timerSound,
            _ => throw new NotSupportedException()
        };

        source.Play();
    }

    #region SETTINGS
    public void TurnSound(bool isSoundOn)
    {
        if (isSoundOn)
        {
            mixerMasterGroup.audioMixer.SetFloat("_MasterVolume", 0);
        }
        else
        {
            mixerMasterGroup.audioMixer.SetFloat("_MasterVolume", -80);
        }
    }

    public void TurnVibration(bool isVibroOn)
    {
        vibroEnabled = isVibroOn;
    }
    #endregion
}
