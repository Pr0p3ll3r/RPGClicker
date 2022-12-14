using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Sprite soundOn, soundOff, vibrationOn, vibrationOff, musicOn, musicOff;
    [SerializeField] private Image soundImage, vibrationImage, musicImage;
    [SerializeField] private AudioMixer audioMixer;

    void Start()
    {
        SetSounds();
        SetMusic();
        SetVibration();
    }

    public void ChangeSounds()
    {
        bool active = GetSetting("Sounds");
        
        if(active)
        {
            soundImage.sprite = soundOff;
            audioMixer.SetFloat("sfxVolume", -80);
            ChangeSetting("Sounds", 0);
        }
        else
        {
            soundImage.sprite = soundOn;
            audioMixer.SetFloat("sfxVolume", 0);
            ChangeSetting("Sounds", 1);
        }
    }

    public void ChangeMusic()
    {
        bool active = GetSetting("Music");

        if (active)
        {
            musicImage.sprite = musicOff;
            audioMixer.SetFloat("musicVolume", -80);
            ChangeSetting("Music", 0);
        }
        else
        {
            musicImage.sprite = musicOn;
            audioMixer.SetFloat("musicVolume", 0);
            ChangeSetting("Music", 1);
        }
    }

    public void ChangeVibration()
    {
        bool active = GetSetting("vibration");
        
        if(active)
        {
            vibrationImage.sprite = vibrationOff;
            ChangeSetting("vibration", 0);
        }
        else
        {
            vibrationImage.sprite = vibrationOn;
            ChangeSetting("vibration", 1);
        }
    }

    public static bool GetSetting(string name)
    {
        return PlayerPrefs.GetInt(name, 1) == 1 ? true : false;
    }

    private void ChangeSetting(string name, int state)
    {
        PlayerPrefs.SetInt(name, state);
    }

    private void SetSounds()
    {
        bool active = GetSetting("Sounds");
        
        if(active)
        {
            soundImage.sprite = soundOn;
            audioMixer.SetFloat("sfxVolume", 0);
        }
        else
        {
            soundImage.sprite = soundOff;
            audioMixer.SetFloat("sfxVolume", -80);
        }
    }

    private void SetMusic()
    {
        bool active = GetSetting("Music");

        if (active)
        {
            musicImage.sprite = musicOn;
            audioMixer.SetFloat("musicVolume", 0);
        }
        else
        {
            musicImage.sprite = musicOff;
            audioMixer.SetFloat("musicVolume", -80);
        }
    }

    private void SetVibration()
    {
        bool active = GetSetting("vibration");

        if (active)
        {
            vibrationImage.sprite = vibrationOn;
        }
        else
        {
            vibrationImage.sprite = vibrationOff;
        }
    }
}
