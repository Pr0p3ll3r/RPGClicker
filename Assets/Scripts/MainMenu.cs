using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private GameObject mainTab;
    [SerializeField] private GameObject optionsTab;

    private void Start()
    {
        SoundManager.Instance.Play("Theme");
        mainTab.SetActive(true);
        optionsTab.SetActive(false);
    }

    public void PlayGame()
    {
        LevelLoader.Instance.LoadScene(1);
    }

    public void OpenTab(GameObject tab)
    {
        CloseTabs();
        tab.SetActive(true);
        ClickSound();
    }

    private void CloseTabs()
    {
        mainTab.SetActive(false);
        optionsTab.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlayOneShot("Hover");
    }

    public void ClickSound()
    {
        SoundManager.Instance.PlayOneShot("Click");
    }
}
