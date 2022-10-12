using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    public Button upgradeButton;
    public static int upgradeLvl = 0;
    public TextMeshProUGUI needText;

    // int[] upgradePrice = new int[100];

    private void Start()
    {


        //for (int i = 1; i < 100; i++)
        //{
        //    //upgradePrice[i] = (int)(100 * Mathf.Pow(1.15f, i));
        //    upgradePrice[i] = 100 * i;
        //}
    }

    void Update()
    {
        //if (GameManagerClicker.number >= (upgradeLvl + 1) * 100)
        //    upgradeButton.interactable = true;
        //else
        //    upgradeButton.interactable = false;

        needText.text = ((upgradeLvl + 1) * 100).ToString();

    }

    public void Upgrade()
    {
        //GameManagerClicker.number -= (upgradeLvl + 1) * 100;
        upgradeLvl++;
    }
}
