using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Sprite soundOn, soundOff;
    [SerializeField] private Image soundImage;
    [SerializeField] private AudioMixer audioMixer;

    void Start()
    {
        SetSounds();
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

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
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
}
