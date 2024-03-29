using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [field: SerializeField]
    public PlayerData Data { get; set; } = new PlayerData();

    private PlayerUI hud;
    private LevelSystem ls;
    private GameManager GameManager => GameManager.Instance;
    private PlayerEquipment Equipment => PlayerEquipment.Instance;
    private PlayerInventory Inventory => PlayerInventory.Instance;

    public Pet[] MyPets { get; private set; } = new Pet[2];
    [SerializeField] private Transform petList;
    private PetInfo[] petsInfo;

    [SerializeField] private Transform popupPosition;

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
        petsInfo = petList.GetComponentsInChildren<PetInfo>();
        RefreshPetList();
        Unlock2ndPetSlot();
        LoadHealthRegeneration();
    }

    private void OnEnable()
    {
        RebirthSystem.OnRebirth += Reborn;
    }

    private void OnDisable()
    {
        RebirthSystem.OnRebirth -= Reborn;
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
        if (Data.IsFullHP() || IsDead) return;

        if (regeneration > 0)
        {
            regeneration -= Time.deltaTime;
            hud.RefreshRegenerationTimer($"Regenerate in {Mathf.FloorToInt(regeneration + 1):00}");
        }
        else
        {
            Data.currentHealth++;
            hud.UpdateHealthBar();
            regeneration = healthRegenerationTime;
            hud.RefreshRegenerationTimer("");
        }          
    }

    public void Attack()
    {
        if (IsDead || Enemy.IsDead) return;

        int damage = Data.damage.GetValue();
        int enemyDefense = Enemy.Data.defense;
        Debug.Log("Your Damage: " + damage + " Enemy Defense: " + enemyDefense);
        DamagePopup damagePopup = GameManager.damagePopupPooler.Get().GetComponent<DamagePopup>();
        damagePopup.transform.position = popupPosition.position;
        bool crit = Utils.Critical(Data.criticalDamage.GetValue(), Data.criticalRate.GetValue(), ref damage, Data.maxCriticalRate.GetValue());
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
        int hpSteal = Mathf.CeilToInt(damage * (float)Data.hpSteal.GetValue()/100);
        hpSteal = Mathf.Clamp(hpSteal, 0, Data.hpStealLimit.GetValue());
        Data.currentHealth += hpSteal;
        Data.currentHealth = Mathf.Clamp(Data.currentHealth, 0, Data.health.GetValue());
        hud.UpdateHealthBar();
        if(Data.IsFullHP())
        {
            hud.RefreshRegenerationTimer("");
            regeneration = healthRegenerationTime;
        }           
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        SoundManager.Instance.Play("PlayerHurt");
        Data.currentHealth -= damage;
        hud.ShowVignette();      
        hud.UpdateHealthBar();
        if(regeneration == 0)
            regeneration = healthRegenerationTime;

        if (Data.currentHealth <= 0)
        {
            Die();
        }
    }

    public void Reward(int exp, int gold)
    {
        exp += Mathf.FloorToInt((float)Data.expBonus.GetValue()/100 * exp);
        gold += Mathf.FloorToInt((float)Data.goldBonus.GetValue()/100 * gold);
        ls.GetExp(exp);
        Inventory.ChangeGoldAmount(gold);
        for (int i = 0; i < MyPets.Length; i++)
        {
            if (MyPets[i] != null)
                MyPets[i].GetExp(exp / 2, petsInfo[i]);
        }
        Data.killedMonsters++;
    }

    public void Die()
    {
        //SoundManager.Instance.Play("PlayerDeath");
#if UNITY_ANDROID
        InitializeAds.Instance.ShowInterstitialAd();
#endif
        IsDead = true;
        hud.ShowRevivePanel();
        GameManager.Instance.PlayerDeath();
        regeneration = healthRegenerationTime;
        hud.RefreshRegenerationTimer("");
        Inventory.ChangeGoldAmount((int)(-Inventory.data.Gold / 2));
    }

    public void Revive()
    {
        IsDead = false;
        Data.currentHealth = Data.health.GetValue();
        hud.UpdateHealthBar();
    }

    private void Reborn()
    {
        IsDead = false;
        Data = Data.Reborn(Data);
        //Clear pets
        for (int i = 0; i < MyPets.Length; i++)
        {
            if (MyPets[i] == null) continue;

            MyPets[i].RemoveStats(Data);
            MyPets[i] = null;
        }
        //Clear Inventory
        Inventory.ChangeGoldAmount((int)-Inventory.data.Gold);
        for (int i = 0; i < Inventory.slots.Length; i++)
        {
            Inventory.slots[i].item = null;
            Inventory.slots[i].amount = 0;
        }
        Inventory.RefreshUI();
        //Clear Equipment
        for (int i = 0; i < Equipment.Slots.Length; i++)
        {
            if (Equipment.Slots[i].item == null) continue;

            Equipment.Slots[i].item.RemoveStats(Data);
            Equipment.Slots[i].item = null;
        }
        Equipment.RefreshUI();
        Data.currentHealth = Data.health.GetValue();
        hud.UpdateLevel();
        hud.UpdateHealthBar();
        playerInfo.RefreshStats();
        regeneration = healthRegenerationTime;
        hud.RefreshRegenerationTimer("");
    }

    public bool Equip(Equipment item)
    {
        if (item.lvlRequired <= Data.level)
        {
            Inventory.RemoveItem(item);
            Equipment.EquipItem(item, out Equipment previousItem);
            item.AddStats(Data);
            if (previousItem != null)
            {
                Inventory.AddItem(previousItem, 1);
                previousItem.RemoveStats(Data);
            }
        }
        else
        {
            GameManager.Instance.ShowText("Level is too low", Color.red);
            return false;
        }
        playerInfo.RefreshStats();
        Equipment.RefreshUI();
        hud.UpdateHealthBar();
        return true;
    }

    public bool Unequip(Equipment item)
    {
        if (!Inventory.IsFull())
        {
            Equipment.UnequipItem(item);
            Inventory.AddItem(item, 1);
            item.RemoveStats(Data);
        }
        else
        {
            GameManager.Instance.ShowText("Inventory is full", Color.red);
            return false;
        }
        playerInfo.RefreshStats();
        Equipment.RefreshUI();
        hud.UpdateHealthBar();
        return true;
    }

    public void EquipPet(Pet pet)
    {
        if (MyPets.Length != Data.maxPets.GetValue())
        {
            for (int i = 0; i < Data.maxPets.GetValue(); i++)
            {
                if (MyPets[i] == null)
                {
                    MyPets[i] = pet;
                    break;
                }
            }
            Inventory.RemoveItem(pet);
            RefreshPetList();
            pet.AddStats(Data);
            hud.UpdateHealthBar();
            playerInfo.RefreshStats();
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
            for (int i = 0; i < MyPets.Length; i++)
            {
                if (MyPets[i] == pet)
                {
                    MyPets[i] = null;
                    break;
                }
            }
            Inventory.AddItem(pet, 1);
            pet.RemoveStats(Data);
            hud.UpdateHealthBar();
            playerInfo.RefreshStats();
        }
        else
        {
            GameManager.Instance.ShowText("Inventory is full", Color.red);
        }
    }

    public void RefreshPetList()
    {
        for (int i = 0; i < MyPets.Length; i++)
        {
            if (MyPets[i] != null)
                petsInfo[i].SetUp(MyPets[i]);
        }
    }

    public bool CanUseIt(Equipment eq)
    {
        if (Data.level < eq.lvlRequired) return false;

        for (int i = 0; i < eq.canBeUsedBy.Length; i++)
        {
            if (Data.playerClass == eq.canBeUsedBy[i])
            {
                return true;
            }
        }
        return false;
    }
    
    public void Unlock2ndPetSlot()
    {
        if(Data.maxPets.GetValue() == 2)
            petsInfo[1].UnlockSlot();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("HealthRegeneration", regeneration);
        PlayerPrefs.SetString("LastTime", DateTime.UtcNow.ToString());
    }

    private void LoadHealthRegeneration()
    {
        regeneration = PlayerPrefs.GetFloat("HealthRegeneration", healthRegenerationTime);
        if (PlayerPrefs.HasKey("LastTime"))
        {
            DateTime lastTime = DateTime.Parse(PlayerPrefs.GetString("LastTime"));
            float seconds = (float)DateTime.UtcNow.Subtract(lastTime).TotalSeconds;
            //Debug.Log("Passed: " + seconds);
            float left = seconds;
            regeneration -= seconds;
            if (regeneration <= 0)
            {
                int regenerated = 1 + Mathf.FloorToInt(left / healthRegenerationTime);
                //Debug.Log("Regenerated: " + regenerated + " times");
                left -= healthRegenerationTime * (regenerated - 1);
                Data.currentHealth += regenerated;
                Data.currentHealth = Mathf.Clamp(Data.currentHealth, 0, Data.health.GetValue());
                regeneration = left;
            }
        }
        hud.UpdateHealthBar();
    }
}
