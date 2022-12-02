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
    public EnemyData data;
    [SerializeField] private Image healthBar;

    private Player player;
    private bool triggered;
    private float attackRate;
    private int currentHealth;
    public bool IsDead { get; private set; }

    private void Start()
    {
        player = Player.Instance;
        attackRate = 5f;
    }

    public void SetUp(EnemyData newEnemy)
    {
      
        data = newEnemy;
        animator.Play("EnemyIn");
        triggered = false;
        currentHealth = data.health;
        enemyName.text = $"Lvl{data.level} {data.enemyName}";
        attackRate = 5f;
        UpdateHealthBar();
        animator.Play(data.enemyName);
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
        if (IsDead) return;

        currentHealth -= damage;
        animator.Play("EnemyHit", -1, 0f);
        UpdateHealthBar();
        triggered = true;

        if (currentHealth <= 0)
        {
            IsDead = true;
            GameManager.Instance.Reward(data);
            triggered = false;
            animator.Play("EnemyOut");
        }
    }

    public void Spawned()
    {
        IsDead = false;
    }

    public void SpawnNextEnemy()
    {
        GameManager.Instance.NextEnemy();
    }

    void Attack()
    {
        if (IsDead) return;

        int damage = data.damage;
        Utils.Critical(data.criticalChance, data.criticalDamage, ref damage);
        damage -= player.data.defense.GetValue();
        damage = Mathf.Clamp(damage, 1, int.MaxValue);
        player.TakeDamage(damage);
        attackRate = 5f;
    }

    public void UpdateHealthBar()
    {
        float ratio = (float)currentHealth / data.health;
        healthBar.fillAmount = ratio;
    }
}
