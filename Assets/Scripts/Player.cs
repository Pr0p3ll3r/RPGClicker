using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public PlayerData data = new PlayerData();

    private PlayerUI hud;
    private LevelSystem ls;
    private GameManager GameManager => GameManager.Instance;
    private PlayerEquipment Equipment => PlayerEquipment.Instance;
    private PlayerInventory Inventory => PlayerInventory.Instance;

    public Pet[] myPets = new Pet[2];
    [SerializeField] private Transform petList;
    private PetInfo[] petsInfo;

    [SerializeField] private Transform popupPosition;

    private PlayerInfo playerInfo;
    private Enemy Enemy => Enemy.Instance;
    public bool IsDead { get; private set; }

    private void Start()
    {
        hud = GetComponent<PlayerUI>();
        playerInfo = GetComponent<PlayerInfo>();
        ls = GetComponent<LevelSystem>();
        data.currentHealth = data.health.GetValue();
        hud.UpdateHealthBar(data.currentHealth, data.health.GetValue());
        petsInfo = petList.GetComponentsInChildren<PetInfo>();
        RefreshPetList();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(1);
        }
#endif
    }

    public void Attack()
    {
        if (IsDead || Enemy.IsDead) return;

        int damage = data.damage.GetValue();
        int enemyDefense = Enemy.data.defense;
        Debug.Log("Your Damage: " + damage + " Enemy Defense: " + enemyDefense);
        DamagePopup damagePopup = GameManager.damagePopupPooler.Get().GetComponent<DamagePopup>();
        damagePopup.transform.position = popupPosition.position;
        bool crit = Utils.Critical(data.criticalRate.GetValue(), data.criticalDamage.GetValue(), ref damage);
        damage -= enemyDefense;
        damage = Mathf.Clamp(damage, 1, int.MaxValue);
        Debug.Log($"Damage: {damage} Crit: {crit}");
        damagePopup.Setup(damage, crit);
        damagePopup.gameObject.SetActive(true);
        Enemy.TakeDamage(damage);
        SoundManager.Instance.PlayOneShot("Hit");
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        SoundManager.Instance.Play("PlayerHurt");
        data.currentHealth -= damage;
        hud.ShowVignette();      
        hud.UpdateHealthBar(data.currentHealth, data.health.GetValue());

        if (data.currentHealth <= 0)
        {
            Die();
        }
    }

    public void Reward(int exp, int gold)
    {
        exp += data.expBonus.GetValue() * exp;
        gold += data.goldBonus.GetValue() * gold;
        ls.GetExp(exp);
        Inventory.AddGold(gold);

        for (int i = 0; i < myPets.Length; i++)
        {
            if (myPets[i] != null)
                myPets[i].GetExp(exp / 2, petsInfo[i]);
        }
    }

    public void Die()
    {
        //SoundManager.Instance.Play("PlayerDeath");
        IsDead = true;
        hud.ShowRevivePanel();
        GameManager.Instance.PlayerDeath();
    }

    public void Revive()
    {
        IsDead = false;
        data.currentHealth = data.health.GetValue();
        hud.UpdateHealthBar(data.currentHealth, data.health.GetValue());
    }

    public bool Equip(Equipment item)
    {
        Equipment previousItem;
        if (item.lvlRequired <= data.level)
        {
            Inventory.RemoveItem(item);
            Equipment.EquipItem(item, out previousItem);
            item.AddStats(data);
            if (previousItem != null)
            {
                Inventory.AddItem(previousItem, 1);
                previousItem.RemoveStats(data);
            }
        }
        else
        {
            GameManager.Instance.ShowText("Level is too low", new Color32(255, 65, 52, 255));
            return false;
        }
        playerInfo.RefreshStats();
        Equipment.RefreshUI();
        return true;
    }

    public bool Unequip(Equipment item)
    {
        if (!Inventory.IsFull())
        {
            Equipment.UnequipItem(item);
            Inventory.AddItem(item, 1);
            item.RemoveStats(data);
        }
        else
        {
            GameManager.Instance.ShowText("Inventory is full", new Color32(255, 65, 52, 255));
            return false;
        }
        playerInfo.RefreshStats();
        Equipment.RefreshUI();
        return true;
    }

    public void EquipPet(Pet pet)
    {
        if (myPets.Length != data.maxPets)
        {
            for (int i = 0; i < data.maxPets; i++)
            {
                if (myPets[i] == null)
                {
                    myPets[i] = pet;
                    break;
                }
            }
            Inventory.RemoveItem(pet);
            Inventory.RefreshUI();
            RefreshPetList();
            pet.AddStats(data);
        }
        else
        {
            GameManager.Instance.ShowText("Pet slots are full", new Color32(255, 65, 52, 255));
        }
    }

    public void UnequipPet(Pet pet)
    {
        if (!Inventory.IsFull())
        {
            for (int i = 0; i < myPets.Length; i++)
            {
                if (myPets[i] == pet)
                {
                    myPets[i] = null;
                    break;
                }
            }
            Inventory.AddItem(pet, 1);
            pet.RemoveStats(data);
        }
        else
        {
            GameManager.Instance.ShowText("Inventory is full", new Color32(255, 65, 52, 255));
        }
    }

    public void RefreshPetList()
    {
        for (int i = 0; i < myPets.Length; i++)
        {
            if (myPets[i] != null)
                petsInfo[i].SetUp(myPets[i]);
        }
    }

    public bool CanUseIt(Equipment eq)
    {
        if (data.level < eq.lvlRequired) return false;

        for (int i = 0; i < eq.canBeUsedBy.Length; i++)
        {
            if (data.playerClass == eq.canBeUsedBy[i])
            {
                return true;
            }
        }
        return false;
    }
}
