using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public PlayerData data;

    private PlayerUI hud;
    private LevelSystem ls;
    private GameManager GameManager => GameManager.Instance;
    private PlayerEquipment Equipment => PlayerEquipment.Instance;
    private PlayerInventory Inventory => PlayerInventory.Instance;

    public Pet[] myPets = new Pet[2];
    [SerializeField] private Transform petList;
    private PetInfo[] petsInfo;

    [SerializeField] private Transform popupPosition;

    [SerializeField] private TextMeshProUGUI healthRegenerationTimer;
    [SerializeField] private float healthRegenerationTime = 30;

    private float regeneration;
    private PlayerInfo playerInfo;
    private Enemy Enemy => Enemy.Instance;
    public bool IsDead { get; private set; }
 
    private void Start()
    {
        hud = GetComponent<PlayerUI>();
        playerInfo = GetComponent<PlayerInfo>();
        ls = GetComponent<LevelSystem>();
        hud.UpdateHealthBar();
        petsInfo = petList.GetComponentsInChildren<PetInfo>();
        RefreshPetList();
        regeneration = PlayerPrefs.GetFloat("HealthRegeneration", healthRegenerationTime);
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Attack();
        }
#endif
        RegenerateHealth();
    }

    private void RegenerateHealth()
    {
        if (data.IsFullHP() || IsDead) return;

        if (regeneration > 0)
        {
            regeneration -= Time.deltaTime;
            healthRegenerationTimer.text = $"Regenerate in {Mathf.FloorToInt(regeneration + 1)}s";
        }
        else
        {
            data.currentHealth++;
            hud.UpdateHealthBar();
            regeneration = healthRegenerationTime;
            healthRegenerationTimer.text = "";
        }          
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
        HealthSteal(damage);
    }

    private void HealthSteal(int damage)
    {
        int hpSteal = Mathf.CeilToInt(damage * (float)data.hpSteal.GetValue()/100);
        hpSteal = Mathf.Clamp(hpSteal, 0, data.hpStealLimit.GetValue());
        data.currentHealth += hpSteal;
        data.currentHealth = Mathf.Clamp(data.currentHealth, 0, data.health.GetValue());
        hud.UpdateHealthBar();
        if(data.IsFullHP())
        {
            healthRegenerationTimer.text = "";
            regeneration = healthRegenerationTime;
        }           
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        SoundManager.Instance.Play("PlayerHurt");
        data.currentHealth -= damage;
        hud.ShowVignette();      
        hud.UpdateHealthBar();
        if(regeneration == 0)
            regeneration = healthRegenerationTime;

        if (data.currentHealth <= 0)
        {
            Die();
        }
    }

    public void Reward(int exp, int gold)
    {
        exp += Mathf.FloorToInt((float)data.expBonus.GetValue()/100 * exp);
        gold += Mathf.FloorToInt((float)data.goldBonus.GetValue()/100 * gold);
        ls.GetExp(exp);
        Inventory.ChangeGoldAmount(gold);
        for (int i = 0; i < myPets.Length; i++)
        {
            if (myPets[i] != null)
                myPets[i].GetExp(exp / 2, petsInfo[i]);
        }
        data.killedMonsters++;
    }

    public void Die()
    {
        //SoundManager.Instance.Play("PlayerDeath");
        IsDead = true;
        hud.ShowRevivePanel();
        GameManager.Instance.PlayerDeath();
        regeneration = healthRegenerationTime;
        healthRegenerationTimer.text = "";
        Inventory.ChangeGoldAmount(-Inventory.data.gold / 2);
    }

    public void Revive()
    {
        IsDead = false;
        data.currentHealth = data.health.GetValue();
        hud.UpdateHealthBar();
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
            GameManager.Instance.ShowText("Level is too low", Color.red);
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
            GameManager.Instance.ShowText("Inventory is full", Color.red);
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
            GameManager.Instance.ShowText("Pet slots are full", Color.red);
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
            GameManager.Instance.ShowText("Inventory is full", Color.red);
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

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("HealthRegeneration", regeneration);
    }
}
