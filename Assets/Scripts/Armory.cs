using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Armory : MonoBehaviour
{
    [SerializeField] private GameObject mainButtons;

    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private GameObject blueprintListItem;
    [SerializeField] private GameObject warningBlueprintText;
    [SerializeField] private Transform listOfBlueprint;

    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject warningUpgradeText;
    [SerializeField] private GameObject upgradeListItem;
    [SerializeField] private Animation infoAnimation;
    [SerializeField] private Transform listOfItems;

    private void OnEnable()
    {
        mainButtons.SetActive(true);
        craftingPanel.SetActive(false);
        upgradePanel.SetActive(false);
    }

    public void Return()
    {
        mainButtons.SetActive(true);
        craftingPanel.SetActive(false);
        upgradePanel.SetActive(false);
    }

    public void OpenCraftingPanel()
    {
        warningBlueprintText.SetActive(false);

        int i;
        for (i = 0; i < listOfBlueprint.childCount; i++)
        {
            Destroy(listOfBlueprint.GetChild(i).gameObject);
        }

        i = 0;
        foreach (Item item in Inventory.Instance.info.items)
        {
            if (item.GetType() == typeof(Blueprint))
            {
                Blueprint b = (Blueprint)item;
                i++;
                GameObject newRecipe = Instantiate(blueprintListItem, listOfBlueprint);
                newRecipe.GetComponent<BlueprintInfo>().SetUp(b);
                newRecipe.transform.Find("ButtonCraft").GetComponent<Button>().onClick.AddListener(delegate { Craft(b); });
            }
        }

        if (i == 0)
        {
            warningBlueprintText.SetActive(true);
        }

        mainButtons.SetActive(false);
        craftingPanel.SetActive(true);
    }

    public void Craft(Blueprint b)
    {
        if (!Inventory.Instance.CheckResources(b))
        {
            GameManager.Instance.ShowText("You don't have enough materials", new Color32(255, 65, 52, 255));
            return;
        }
        Equipment resultItem = Loot.Craft(b.resultItem);
        ShowText("Crafted " + resultItem.itemName, true);
        Inventory.Instance.RemoveItem(b, 1);
        Inventory.Instance.AddItem(resultItem);
        OpenCraftingPanel();
    }

    public void OpenUpgradePanel()
    {
        warningUpgradeText.SetActive(false);

        int i;
        for (i = 0; i < listOfItems.childCount; i++)
        {
            Destroy(listOfItems.GetChild(i).gameObject);
        }

        i = 0;
        foreach (Item item in Inventory.Instance.info.items)
        {
            Equipment eq;
            if (item.itemType == ItemType.Equipment)
            {
                eq = (Equipment)item;
            }               
            else continue;

            if (eq.rarity == EquipmentRarity.Legendary)
            {
                if (eq.level == 9) continue;

                i++;
                GameObject itemToUpgrade = Instantiate(upgradeListItem, listOfItems);
                itemToUpgrade.GetComponent<UpgradeInfo>().SetUp(eq);
                EquipmentUpgrade upgradeItem = Library.FindItem(eq);
                itemToUpgrade.transform.Find("ButtonUpgrade").GetComponent<Button>().onClick.AddListener(delegate { Upgrade(eq, upgradeItem); });
            }
        }
        foreach (Item item in Player.Instance.info.equipment)
        {
            Equipment eq;
            if (item.itemType == ItemType.Equipment)
            {
                eq = (Equipment)item;
            }
            else continue;

            if (eq.rarity == EquipmentRarity.Legendary)
            {
                if (eq.level == 9) continue;

                i++;
                GameObject itemToUpgrade = Instantiate(upgradeListItem, listOfItems);
                itemToUpgrade.GetComponent<UpgradeInfo>().SetUp(eq);
                EquipmentUpgrade upgradeItem = Library.FindItem(eq);
                itemToUpgrade.transform.Find("ButtonUpgrade").GetComponent<Button>().onClick.AddListener(delegate { Upgrade(eq, upgradeItem); });
            }
        }

        if (i == 0)
        {
            warningUpgradeText.SetActive(true);
        }

        mainButtons.SetActive(false);
        upgradePanel.SetActive(true);
    }

    public void Upgrade(Equipment eq, EquipmentUpgrade upgrade)
    {
        if (!Inventory.Instance.CheckResources(upgrade, eq.level))
        {
            GameManager.Instance.ShowText("You don't have enough materials", new Color32(255, 65, 52, 255));
            return;
        }

        float chance = 1 - eq.level * 0.1f;

        if (Chance(chance))
        {
            ShowText("Upgrade successful", true);
            Upgrade(eq);
        }
        else
        {
            ShowText("Upgrade failed", false);
        }     
        OpenUpgradePanel();
    }

    void Upgrade(Equipment eq)
    {
        switch(eq.equipSlot)
        {
            case EquipmentSlot.Weapon:
                eq.damage = (int)(eq.damage * 1.1f);
                break;
            case EquipmentSlot.Helmet:
            case EquipmentSlot.Chestplate:
            case EquipmentSlot.Hands:
            case EquipmentSlot.Leggins:
            case EquipmentSlot.Feet:
            case EquipmentSlot.Shield:
                eq.defense = (int)(eq.defense * 1.1f);
                break;
            case EquipmentSlot.Necklace:
            case EquipmentSlot.Ring0:
                eq.strength = (int)(eq.strength * 1.1f);
                eq.criticalDamage = (int)(eq.criticalDamage * 1.1f);
                eq.vitality = (int)(eq.vitality * 1.1f);
                eq.criticalChance = (int)(eq.criticalChance * 1.1f);
                break;
        }
        eq.level++;
    }

    bool Chance(float chance)
    {
        float random = Random.value;
        if (random <= chance)
        {
            return true;
        }
        else return false;
    }

    void ShowText(string text, bool success)
    {
        infoAnimation.GetComponent<TextMeshProUGUI>().text = text;
        if(success)
            infoAnimation.GetComponent<TextMeshProUGUI>().color = Color.green;
        else
            infoAnimation.GetComponent<TextMeshProUGUI>().color = Color.red;
        infoAnimation.Stop();
        infoAnimation.Play();
    }
}
