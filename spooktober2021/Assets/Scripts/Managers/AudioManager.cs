using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region instances
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("AudioManager Instance not found");

            return instance;
        }
    }
    #endregion

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource source2D;
    [SerializeField] private List<AudioSource> soundsSources;

    [SerializeField] private AudioMixer mainMixer;

    [Header("Groups Names")]
    [SerializeField] private string masterVolParam = "MasterVolume";
    [SerializeField] private string musicVolParam = "MusicVolume";
    [SerializeField] private string soundVolParam = "SoundVolume";

    [Header("Sliders")]
    [SerializeField] private Slider mainSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [Space]
    [SerializeField] private float volMultiplier = 30f;


    private bool musicFlag = false;

    #region Clips References

    public enum ClipsTags
    {
        // sounds


        // musics

        MainTheme,
        MainMenu,
        GameOver,
        Win,

    }

    [System.Serializable]
    private struct SoundClips
    {
        public string clipName;
        public AudioClip clip;
    }

    [System.Serializable]
    private struct MusicClips
    {
        public string clipName;
        public AudioClip clip;
    }
    [Header("Clips")]

    [SerializeField] private List<SoundClips> soundClips;
    [SerializeField] private List<MusicClips> musicClips;

    #endregion

    private void Awake()
    {
        instance = this;

        if (!PlayerPrefs.HasKey("masterVolume"))
            PlayerPrefs.SetFloat("masterVolume", 1);
        if (!PlayerPrefs.HasKey("masterMute"))
            PlayerPrefs.SetInt("masterMute", 0);

        if (!PlayerPrefs.HasKey("musicVolume"))
            PlayerPrefs.SetFloat("musicVolume", 1);
        if (!PlayerPrefs.HasKey("musicMute"))
            PlayerPrefs.SetInt("musicMute", 0);

        if (!PlayerPrefs.HasKey("soundsVolume"))
            PlayerPrefs.SetFloat("soundsVolume", 1);
        if (!PlayerPrefs.HasKey("soundsMute"))
            PlayerPrefs.SetInt("soundsMute", 0);


    }

    private void Start()
    {
        if (mainSlider != null)
        {
            mainSlider.value = PlayerPrefs.GetFloat("masterVolume");
            OnMainSliderValueChanged(mainSlider.value);
            mainSlider.onValueChanged.AddListener(OnMainSliderValueChanged);
        }

        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            OnMusicSliderValueChanged(musicSlider.value);
            musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        }

        if (soundSlider != null)
        {
            soundSlider.value = PlayerPrefs.GetFloat("soundsVolume");
            OnSoundSliderValueChanged(soundSlider.value);
            soundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
        }



        if (PlayerPrefs.GetInt("masterMute") == 1)
            MuteAudio("masterMute", true);
        if (PlayerPrefs.GetInt("musicMute") == 1)
            MuteAudio("musicMute", true);
        if (PlayerPrefs.GetInt("soundsMute") == 1)
            MuteAudio("soundsMute", true);
    }

    public void MuteA(string audio)
    {
        if (PlayerPrefs.GetInt(audio) == 0)
            MuteAudio(audio, true);
        else
            MuteAudio(audio, false);
    }

    public void MuteAudio(string audio, bool mute)
    {
        switch (audio)
        {
            case "masterMute":
                if (mute)
                {
                    mainMixer.SetFloat(masterVolParam, -80);
                    PlayerPrefs.SetInt(audio, 1);
                }
                else
                {
                    OnMainSliderValueChanged(PlayerPrefs.GetFloat("masterVolume"));
                    PlayerPrefs.SetInt(audio, 0);
                }
                break;

            case "musicMute":
                if (mute)
                {
                    mainMixer.SetFloat(musicVolParam, -80);
                    PlayerPrefs.SetInt(audio, 1);
                }
                else
                {
                    OnMusicSliderValueChanged(PlayerPrefs.GetFloat("musicVolume"));
                    PlayerPrefs.SetInt(audio, 0);
                }
                break;

            case "soundsMute":
                if (mute)
                {
                    mainMixer.SetFloat(soundVolParam, -80);
                    PlayerPrefs.SetInt(audio, 1);
                }
                else
                {
                    OnSoundSliderValueChanged(PlayerPrefs.GetFloat("soundsVolume"));
                    PlayerPrefs.SetInt(audio, 0);
                }
                break;

            default:
                Debug.LogError(audio + " not found in switch statement.");
                break;
        }
    }

    private void OnMainSliderValueChanged(float value)
    {
        float newVol = 0;
        if (value > 0)
            newVol = Mathf.Log10(value) * volMultiplier;
        else
            newVol = -80;

        mainMixer.SetFloat(masterVolParam, newVol);
        PlayerPrefs.SetFloat("masterVolume", value);
    }
    private void OnMusicSliderValueChanged(float value)
    {
        float newVol = 0;
        if (value > 0)
            newVol = Mathf.Log10(value) * volMultiplier;
        else
            newVol = -80;

        mainMixer.SetFloat(musicVolParam, newVol);
        PlayerPrefs.SetFloat("musicVolume", value);
    }
    private void OnSoundSliderValueChanged(float value)
    {
        float newVol = 0;
        if (value > 0)
            newVol = Mathf.Log10(value) * volMultiplier;
        else
            newVol = -80;

        mainMixer.SetFloat(soundVolParam, newVol);
        PlayerPrefs.SetFloat("soundsVolume", value);
    }

    #region Sounds and music plays

    #region Sounds

    /// <summary>
    /// Returns a audio clip by name
    /// </summary>
    /// <param name="searchedAudio"></param>
    /// <returns></returns>
    public AudioClip GetAudioClip(ClipsTags searchedAudio)
    {
        foreach (SoundClips sound in soundClips)
        {
            if (sound.clipName.Equals(searchedAudio.ToString()))
                return sound.clip;
        }

        Debug.LogError(searchedAudio + " not found in Audio Clips.");
        return null;
    }

    /// <summary>
    /// Plays a sound in 2D, the audio source should be located in the AudioManager
    /// </summary>
    /// <param name="searchedSound"></param>
    public void Play2DSound(ClipsTags searchedSound)
    {
        foreach (SoundClips sound in soundClips)
        {
            if (sound.clipName.Equals(searchedSound.ToString()))
            {
                source2D.PlayOneShot(sound.clip);
                return;
            }
        }

        Debug.LogError(searchedSound + " not found in Audio Clips.");
    }
    public void Play2DSound(string searchedSound)
    {
        foreach (SoundClips sound in soundClips)
        {
            if (sound.clipName.Equals(searchedSound.ToString()))
            {
                source2D.PlayOneShot(sound.clip);
                return;
            }
        }

        Debug.LogError(searchedSound + " not found in Audio Clips.");
    }

    #endregion

    #region Musics

    /// <summary>
    /// Plays a music based of the actual game state. The audio source should be located in the AudioManager
    /// </summary>
    public void PlayMusic()
    {
        string musicToPlay = GameManager.Instance.GameState.ToString();

        musicSource.Stop();
        musicFlag = false;
        if (musicFlag == false)
        {
            musicFlag = true;
            foreach (MusicClips music in musicClips)
            {
                if (music.clipName.Equals(musicToPlay))
                    musicSource.clip = music.clip;
            }
            if (musicSource.clip != null)
                musicSource.Play();
            else
                Debug.LogError("Music not found for " + "\"" + GameManager.Instance.GameState + "\"" + " state of game.");
        }
    }
    /// <summary>
    /// Plays a chosen music. The audio source should be located in the AudioManager
    /// </summary>
    /// <param name="searchedMusic"></param>
    public void PlayMusic(ClipsTags searchedMusic)
    {
        musicSource.Stop();
        musicFlag = false;
        if (musicFlag == false)
        {
            musicFlag = true;
            foreach (MusicClips music in musicClips)
            {
                if (music.clipName.Equals(searchedMusic.ToString()))
                    musicSource.clip = music.clip;
            }
            if (musicSource.clip != null)
                musicSource.Play();
            else
                Debug.LogError(searchedMusic + " not found in Music Clips.");
        }
    }

    /// <summary>
    /// Stops the playing music
    /// </summary>
    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
            musicFlag = false;
        }
    }

    /// <summary>
    /// Stops the playing music, then play one based of the actual game state
    /// </summary>
    public void StopAndReplay()
    {
        StopMusic();
        PlayMusic();
    }

    #endregion

    #endregion

    #region Volumes



    #endregion
}