using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private GameManager gameManager => GameManager.Instance;
    private PlayerEquipment Equipment => PlayerEquipment.Instance;
    private PlayerInventory Inventory => PlayerInventory.Instance;
    public List<Pet> myPets = new List<Pet>();

    [SerializeField] private Transform popupPosition;

    private PlayerInfo playerInfo;
    private Enemy enemy;
    public bool IsDead { get; private set; }

    private void Start()
    {
        hud = GetComponent<PlayerUI>();
        playerInfo = GetComponent<PlayerInfo>();
        ls = GetComponent<LevelSystem>();
        data.currentHealth = data.maxHealth.GetValue();
        hud.UpdateHealthBar(data.currentHealth, data.maxHealth.GetValue());
        enemy = Enemy.Instance;
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
        if (IsDead || enemy.IsDead) return;

        int damage = data.damage.GetValue();
        int enemyDefense = enemy.data.defense;
        Debug.Log("Your Damage: " + damage + " Enemy Defense: " + enemyDefense);
        DamagePopup damagePopup = gameManager.damagePopupPooler.Get().GetComponent<DamagePopup>();
        damagePopup.transform.position = popupPosition.position;
        bool crit = Utils.Critical(data.criticalRate.GetValue(), data.criticalDamage.GetValue(), ref damage);
        damage -= enemyDefense;
        damage = Mathf.Clamp(damage, 1, int.MaxValue);
        Debug.Log($"Damage: {damage} Crit: {crit}");
        damagePopup.Setup(damage, crit);
        damagePopup.gameObject.SetActive(true);
        enemy.TakeDamage(damage);
        SoundManager.Instance.PlayOneShot("Hit");
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        SoundManager.Instance.Play("PlayerHurt");
        data.currentHealth -= damage;
        hud.ShowVignette();      
        hud.UpdateHealthBar(data.currentHealth, data.maxHealth.GetValue());

        if (data.currentHealth <= 0)
        {
            Die();
        }
    }

    public void Reward(int exp, int gold)
    {
        ls.GetExp(exp);
        Inventory.AddGold(gold);
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
        data.currentHealth = data.maxHealth.GetValue();
        hud.UpdateHealthBar(data.currentHealth, data.maxHealth.GetValue());
    }

    public bool Equip(Equipment item)
    {
        Equipment previousItem;
        if (item.lvlRequired <= data.level)
        {
            PlayerInventory.Instance.RemoveItem(item);
            PlayerEquipment.Instance.EquipItem(item, out previousItem);
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
        }
        else
        {
            GameManager.Instance.ShowText("Inventory is full", new Color32(255, 65, 52, 255));
            return false;
        }
        Equipment.RefreshUI();
        return true;
    }
}
