using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lvl;
    [SerializeField] private TextMeshProUGUI speed;
    [SerializeField] private TextMeshProUGUI luck;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI exp;

    private Pet info;

    public void SetUp(Pet pet)
    {
        info = pet;
        RefreshStats();
    }

    void RefreshStats()
    {
        lvl.text = info.level.ToString();
        UpdateExpBar();
    }

    void UpdateExpBar()
    {
        float ratio = (float)info.exp / (float)(info.expToLvlUp);
        expBar.fillAmount = ratio;
        exp.text = $"{info.exp}/{info.expToLvlUp}";
    }
}
