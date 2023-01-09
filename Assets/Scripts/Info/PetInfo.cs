using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetInfo : MonoBehaviour
{
    [SerializeField] private Image petIcon;
    [SerializeField] private Sprite emptyIcon;
    [SerializeField] private TextMeshProUGUI petName;
    [SerializeField] private ItemStatInfo lvl;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI exp;
    [SerializeField] private Transform statParent;
    [SerializeField] private GameObject statPrefab;
    [SerializeField] private Button unequipButton;

    private Pet pet;

    public void SetUp(Pet _pet)
    {
        pet = _pet;
        petName.text = pet.itemName;
        petIcon.sprite = pet.icon;
        unequipButton.onClick.RemoveAllListeners();
        unequipButton.onClick.AddListener(delegate { Unequip(); });
        unequipButton.gameObject.SetActive(true);
        expBar.transform.parent.gameObject.SetActive(true);
        RefreshStats();
    }

    public void RefreshStats()
    {
        foreach (Transform t in statParent)
            Destroy(t.gameObject);

        foreach (StatBonus stat in pet.scrollsStat)
        {
            if (stat == null) continue;

            ItemStatInfo statInfo = Instantiate(statPrefab, statParent).GetComponent<ItemStatInfo>();
            statInfo.SetUp($"{Utils.GetNiceName(stat.stat)}:", $"{stat.values[1]}");
        }

        UpdateExpBar();
    }

    private void Clear()
    {
        unequipButton.gameObject.SetActive(false);
        unequipButton.onClick.RemoveAllListeners();
        foreach (Transform stat in statParent)
            Destroy(stat.gameObject);
        lvl.SetUp("", "");
        exp.text = "0/0";
        petName.text = "Slot for Pet";
        petIcon.sprite = emptyIcon;
        expBar.transform.parent.gameObject.SetActive(false);
    }

    private void Unequip()
    {
        Player.Instance.UnequipPet(pet);
        Clear();
    }

    public void UpdateExpBar()
    {
        if(pet.LevelMaxed())
        {
            expBar.fillAmount = 1;
            exp.text = $"MAXED";
        }
        else
        {
            int requireExp = Pet.BASE_REQUIRE_EXP * pet.level;
            float ratio = (float)pet.exp / requireExp;
            expBar.fillAmount = ratio;
            exp.text = $"{pet.exp}/{requireExp}";
        }
        lvl.SetUp("Level:", $"{pet.level}");
    }
}
