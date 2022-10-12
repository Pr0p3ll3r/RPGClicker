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

    public PlayerInfo info;
    public List<Pet> myPets = new List<Pet>();

    [SerializeField] private Transform equipmentParent;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI exp;
    [SerializeField] private Transform popupPosition;
    [SerializeField] private GameObject vignette;

    private PlayerUI playerUI;
    private Enemy enemy;
    private EquipmentSlotUI[] slots;

    private void Start()
    {
        slots = equipmentParent.GetComponentsInChildren<EquipmentSlotUI>();
        playerUI = GetComponent<PlayerUI>();
        SetUpUI();
        RefreshPlayer();
        enemy = Enemy.Instance;
    }

    public void Attack()
    {
        if (enemy.IsDead()) return;

        int damage = info.damage.GetValue();
        DamagePopup damagePopup = DamagePopupPooler.Instance.Get().GetComponent<DamagePopup>();
        damagePopup.transform.position = popupPosition.position;
        if (Utilities.Critical(info, ref damage))
        {
            damagePopup.Setup(damage, true);
        }
        else
        {
            damagePopup.Setup(damage, false);
        }
        damagePopup.gameObject.SetActive(true);
        damage -= enemy.info.defense.GetValue();
        damage = Mathf.Clamp(damage, 1, int.MaxValue);
        enemy.TakeDamage(damage);
        SoundManager.Instance.PlayOneShot("Hit");
    }

    public void RefreshPlayer()
    {   
        RefreshEquipment();
        UpdateHealthBar();
        UpdateExpBar();
        playerUI.RefreshStats();
    }

    public void SetUpUI()
    {
        playerUI.info = info;
    }

    void RefreshEquipment()
    {
        for (int i = 0; i < info.equipment.Length; i++)
        {
            if (info.equipment[i] != null)
                slots[i].AddEquipment(info.equipment[i]);
            else
                slots[i].ClearSlot();
        }
    }

    void UpdateHealthBar()
    {
        float ratio = (float)info.currentHealth / (float)info.maxHealth;
        healthBar.fillAmount = ratio;
        health.text = $"{info.currentHealth}/{info.maxHealth}";
    }

    public void TakeDamage(int damage)
    {
        info.TakeDamage(damage);
        //SoundManager.Instance.PlayOneShot("Hit");
        StartCoroutine(FadeToZeroAlpha());
        UpdateHealthBar();
    }

    public void AddExp(int exp)
    {
        info.AddExp(exp);
        UpdateExpBar();
    }

    void UpdateExpBar()
    {
        float ratio = (float)info.exp / (float)info.expToLvl;
        expBar.fillAmount = ratio;
        exp.text = $"{info.exp}/{info.expToLvl}";
        level.text = info.level.ToString();
    }

    public void ShowDeadText()
    {
        vignette.GetComponent<CanvasGroup>().alpha = 1f;
    }

    private IEnumerator FadeToZeroAlpha()
    {
        vignette.GetComponent<CanvasGroup>().alpha = 0.5f;

        while (vignette.GetComponent<CanvasGroup>().alpha > 0.0f)
        {
            vignette.GetComponent<CanvasGroup>().alpha -= (Time.deltaTime / 4f);
            yield return null;
        }
    }

    public void AddMonsterKill()
    {
        info.killedMonsters++;
        playerUI.RefreshOtherStats();
    }
}
