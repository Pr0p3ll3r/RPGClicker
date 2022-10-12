using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Image look;
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private Animation anim;
    public EnemyInfo info;
    [SerializeField] private Image healthBar;

    private Player player;
    private bool triggered;
    private bool spawned;
    private float attackRate;
    private bool isDead;

    private void Start()
    {
        player = Player.Instance;
        attackRate = 5f;
        UpdateHealthBar();
    }

    public void SetUp(EnemyInfo newEnemy)
    {
        StopAllCoroutines();
        anim.Play("EnemyIn");
        triggered = false;
        info = newEnemy;
        info.Initialize();
        look.sprite = info.look;
        enemyName.text = info.characterName;
        look.enabled = true;
        attackRate = 5f;
        UpdateHealthBar();
    }

    void Update()
    {     
        if(triggered)
        {
            attackRate -= Time.deltaTime;
            if (attackRate <= 0)
            {
                Attack();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!spawned) return;

        isDead = info.TakeDamage(damage);
        anim.Play("EnemyHit");
        UpdateHealthBar();
        triggered = true;

        if (isDead)
        {
            GameManager.Instance.Reward(info);
            triggered = false;
            spawned = false;
            anim.Play("EnemyOut");         
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void NextEnemy()
    {
        SetUp(GameManager.Instance.NextEnemy());
    }

    public void Spawned()
    {
        spawned = true;
        isDead = false;
    }

    void Attack()
    {
        if (isDead) return;

        int damage = info.damage.GetValue();
        Utilities.Critical(info, ref damage);
        damage -= player.info.defense.GetValue();
        damage = Mathf.Clamp(damage, 1, int.MaxValue);
        player.TakeDamage(damage);
        attackRate = 5f;
    }

    public void UpdateHealthBar()
    {
        float ratio = (float)info.currentHealth / (float)info.maxHealth;
        healthBar.fillAmount = ratio;
    }
}
