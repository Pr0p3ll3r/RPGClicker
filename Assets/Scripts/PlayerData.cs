[System.Serializable]
public class PlayerData
{
    public string nickname;
    public PlayerClass playerClass;
    public int level;
    public int exp;
    public int remainingPoints;
    public Stat strength;
    public Stat intelligence;
    public Stat dexterity;

    public int currentHealth;
    public Stat damage;
    public Stat defense;
    public Stat maxHealth;
    public Stat criticalDamage;
    public Stat criticalRate;
    public Stat maxCriticalRate;
    public Stat accuracy;
    public Stat goldBonus;
    public Stat expBonus;
    public Stat hpSteal;
    public Stat hpStealLimit;
    public Stat twoSlotDropBonus;

    public int maxPets;
    public int currentTowerLevel;

    public PlayerData()
    {
        nickname = "BOB";
        level = 1;
        exp = 0;
        remainingPoints = 0;
        strength = new Stat(0);
        intelligence = new Stat(0);
        dexterity = new Stat(0);
        damage = new Stat(1);
        defense = new Stat(0);
        maxHealth = new Stat(10);
        currentHealth = maxHealth.GetValue();
        criticalDamage = new Stat(20);
        criticalRate = new Stat(5);
        maxCriticalRate = new Stat(50);
        accuracy = new Stat(6);
        goldBonus = new Stat(0);
        expBonus = new Stat(0);
        hpSteal = new Stat(0);
        hpStealLimit = new Stat(0);
        twoSlotDropBonus = new Stat(0);
        maxPets = 1;
        currentTowerLevel = 0;
    }

    public void AddStat(Stats stat, int amount)
    {
        switch (stat)
        {
            case Stats.Damage:
                damage.AddModifier(amount);
                break;
            case Stats.Defense:
                defense.AddModifier(amount);
                break;
            case Stats.Health:
                maxHealth.AddModifier(amount);
                break;
            case Stats.CriticalDamage:
                criticalDamage.AddModifier(amount);
                break;
            case Stats.CriticalRate:
                criticalRate.AddModifier(amount);
                break;
            case Stats.MaxCriticalRate:
                maxCriticalRate.AddModifier(amount);
                break;
            case Stats.Accuracy:
                accuracy.AddModifier(amount);
                break;
            case Stats.HPStealLimit:
                hpStealLimit.AddModifier(amount);
                break;
            case Stats.HPStealPercent:
                hpSteal.AddModifier(amount);
                break;
            case Stats.GoldBonusPercent:
                goldBonus.AddModifier(amount);
                break;
            case Stats.ExpBonusPercent:
                expBonus.AddModifier(amount);
                break;
            case Stats.TwoSlotItemDropPercent:
                twoSlotDropBonus.AddModifier(amount);
                break;
            case Stats.Strength:
                AddStrength(false, amount);
                break;
            case Stats.Intelligence:
                AddIntelligence(false, amount);
                break;
            case Stats.Dexterity:
                AddDexterity(false, amount);
                break;
        }
    }

    public void RemoveStat(Stats stat, int amount)
    {
        switch (stat)
        {
            case Stats.Damage:
                damage.RemoveModifier(amount);
                break;
            case Stats.Defense:
                defense.RemoveModifier(amount);
                break;
            case Stats.Health:
                maxHealth.RemoveModifier(amount);
                break;
            case Stats.CriticalDamage:
                criticalDamage.RemoveModifier(amount);
                break;
            case Stats.CriticalRate:
                criticalRate.RemoveModifier(amount);
                break;
            case Stats.MaxCriticalRate:
                maxCriticalRate.RemoveModifier(amount);
                break;
            case Stats.Accuracy:
                accuracy.RemoveModifier(amount);
                break;
            case Stats.HPStealLimit:
                hpStealLimit.RemoveModifier(amount);
                break;
            case Stats.HPStealPercent:
                hpSteal.RemoveModifier(amount);
                break;
            case Stats.GoldBonusPercent:
                goldBonus.RemoveModifier(amount);
                break;
            case Stats.ExpBonusPercent:
                expBonus.RemoveModifier(amount);
                break;
            case Stats.TwoSlotItemDropPercent:
                twoSlotDropBonus.RemoveModifier(amount);
                break;
            case Stats.Strength:
                RemoveStrength(amount);
                break;
            case Stats.Intelligence:
                RemoveIntelligence(amount);
                break;
            case Stats.Dexterity:
                RemoveDexterity(amount);
                break;
        }
    }

    public void AddStrength(bool points, int amount)
    {
        if(points)
            remainingPoints--;
        strength.AddModifier(amount);
        damage.AddModifier(amount);
        defense.AddModifier(amount);
    }

    public void AddIntelligence(bool points, int amount)
    {
        if(points)
            remainingPoints--;
        intelligence.AddModifier(amount);
        maxHealth.AddModifier(amount);
    }

    public void AddDexterity(bool points, int amount)
    {
        if (points)
            remainingPoints--;
        dexterity.AddModifier(amount);
        damage.AddModifier(amount);
        accuracy.AddModifier(amount);
    }

    public void RemoveStrength(int amount)
    {
        strength.RemoveModifier(amount);
        damage.RemoveModifier(amount);
        defense.RemoveModifier(amount);
    }

    public void RemoveIntelligence(int amount)
    {
        intelligence.RemoveModifier(amount);
        maxHealth.RemoveModifier(amount);
    }

    public void RemoveDexterity(int amount)
    {
        dexterity.RemoveModifier(amount);
        damage.RemoveModifier(amount);
        accuracy.RemoveModifier(amount);
    }
}