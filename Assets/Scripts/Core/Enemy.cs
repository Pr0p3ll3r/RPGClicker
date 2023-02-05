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

    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private Animator animator;
    public EnemyData Data { get; private set; }
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private float attackRate;

    private Player player;
    private bool triggered;
    private float currentAttackRate;
    private int currentHealth;
    public bool IsDead { get; private set; }

    private void Start()
    {
        player = Player.Instance;
        currentAttackRate = attackRate;
    }

    public void SetUp(EnemyData newEnemy)
    {      
        Data = newEnemy;
        animator.Play("EnemyIn");
        triggered = false;
        currentHealth = Data.health;
        enemyName.text = $"Lvl{Data.level} {Data.enemyName}";
        currentAttackRate = attackRate;
        UpdateHealthBar();
        animator.Play(Data.enemyName);
    }

    void Update()
    {     
        if(triggered)
        {
            currentAttackRate -= Time.deltaTime;
            if (currentAttackRate <= 0)
            {
                Attack();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHealth -= damage;
        animator.Play("EnemyHit", -1, 0f);
        UpdateHealthBar();
        triggered = true;

        if (currentHealth <= 0)
        {
            IsDead = true;
            GameManager.Instance.Reward(Data);
            triggered = false;
            animator.Play("EnemyOut");
        }
    }

    //Used after animation 'EnemyIn'
    public void Spawned()
    {
        IsDead = false;
    }

    //Used after animation 'EnemyOut'
    public void SpawnNextEnemy()
    {
        GameManager.Instance.NextEnemy();
    }

    void Attack()
    {
        if (IsDead) return;

        int damage = Data.damage;
        Utils.Critical(Data.criticalDamage, Data.criticalRate, ref damage);
        damage -= player.Data.defense.GetValue();
        damage = Mathf.Clamp(damage, 1, int.MaxValue);
        player.TakeDamage(damage);
        currentAttackRate = attackRate;
    }

    public void UpdateHealthBar()
    {
        float ratio = (float)currentHealth / Data.health;
        health.text = $"{currentHealth}/{Data.health}";
        healthBar.fillAmount = ratio;
    }
}
