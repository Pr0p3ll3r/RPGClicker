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
    public Stat health;
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
    public bool IsFullHP()
    {
        if (currentHealth == health.GetValue())
            return true;
        return false; 
    }

    public PlayerData()
    {
        nickname = "BOB";
        playerClass = (PlayerClass)1;
        level = 1;
        exp = 0;
        remainingPoints = 0;
        maxPets = 1;
        currentTowerLevel = 0;
        strength = new Stat(0);
        intelligence = new Stat(0);
        dexterity = new Stat(0);
        damage = new Stat(1);
        defense = new Stat(0);
        health = new Stat(10);
        currentHealth = health.GetValue();
        criticalDamage = new Stat(20);
        criticalRate = new Stat(5);
        maxCriticalRate = new Stat(50);
        accuracy = new Stat(6);
        goldBonus = new Stat(0);
        expBonus = new Stat(0);
        hpSteal = new Stat(0);
        hpStealLimit = new Stat(0);
        twoSlotDropBonus = new Stat(0);     
    }

    public PlayerData(string _nickname, PlayerClass _playerClass)
    {
        nickname = _nickname;
        playerClass = _playerClass;
        level = 1;
        exp = 0;
        remainingPoints = 0;
        maxPets = 1;
        currentTowerLevel = 0;
        damage = new Stat(0);
        defense = new Stat(0);
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                strength = new Stat(24);
                intelligence = new Stat(3);
                dexterity = new Stat(8);
                damage.AddModifier(24);
                defense.AddModifier(8);
                health = new Stat(56);
                health.AddModifier(24);
                accuracy = new Stat(4);
                break;
            case PlayerClass.Blader:
                strength = new Stat(16);
                intelligence = new Stat(3);
                dexterity = new Stat(16);
                damage.AddModifier(16);
                defense.AddModifier(16);
                health = new Stat(50);
                accuracy = new Stat(6);
                break;
            case PlayerClass.Archer:
                strength = new Stat(6);
                intelligence = new Stat(3);
                dexterity = new Stat(17);
                damage.AddModifier(17);
                defense.AddModifier(6);
                health = new Stat(40);
                accuracy = new Stat(5);
                break;
            case PlayerClass.Wizard:
                strength = new Stat(3);
                intelligence = new Stat(26);
                dexterity = new Stat(6);
                damage.AddModifier(26);
                defense.AddModifier(6);
                health = new Stat(40);
                accuracy = new Stat(5);
                break;
            case PlayerClass.Shielder:
                strength = new Stat(15);
                intelligence = new Stat(3);
                dexterity = new Stat(9);
                damage.AddModifier(15);
                defense.AddModifier(9);
                health = new Stat(45);
                accuracy = new Stat(3);
                break;
        }
        currentHealth = health.GetValue();
        criticalDamage = new Stat(20);
        criticalRate = new Stat(5);
        maxCriticalRate = new Stat(50);        
        goldBonus = new Stat(0);
        expBonus = new Stat(0);
        hpSteal = new Stat(0);
        hpStealLimit = new Stat(0);
        twoSlotDropBonus = new Stat(0);
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
                health.AddModifier(amount);
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
                health.RemoveModifier(amount);
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
        strength.AddModifier(amount);
        if (points)
            remainingPoints--;
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                damage.AddModifier(amount);
                health.AddModifier(amount);
                break;
            case PlayerClass.Blader:
                defense.AddModifier(amount);
                break;
            case PlayerClass.Archer:
                defense.AddModifier(amount);
                break;
            case PlayerClass.Wizard:
                break;
            case PlayerClass.Shielder:
                damage.AddModifier(amount);
                break;
        }
    }

    public void AddIntelligence(bool points, int amount)
    {
        intelligence.AddModifier(amount);
        if(points)
            remainingPoints--;
         switch (playerClass)
         {
            case PlayerClass.Warrior:
                break;
            case PlayerClass.Blader:
                break;
            case PlayerClass.Archer:
                break;
            case PlayerClass.Wizard:
                damage.AddModifier(amount);
                break;
            case PlayerClass.Shielder:
                break;
         }
    }

    public void AddDexterity(bool points, int amount)
    {
        dexterity.AddModifier(amount);
        if (points)
            remainingPoints--;
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                defense.AddModifier(amount);
                break;
            case PlayerClass.Blader:
                damage.AddModifier(amount);
                break;
            case PlayerClass.Archer:
                damage.AddModifier(amount);
                break;
            case PlayerClass.Wizard:
                defense.AddModifier(amount);
                break;
            case PlayerClass.Shielder:
                defense.AddModifier(amount);
                break;
        }
    }

    public void RemoveStrength(int amount)
    {
        strength.RemoveModifier(amount);
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                damage.RemoveModifier(amount);
                health.RemoveModifier(amount);
                break;
            case PlayerClass.Blader:
                defense.RemoveModifier(amount);
                break;
            case PlayerClass.Archer:
                defense.RemoveModifier(amount);
                break;
            case PlayerClass.Wizard:
                break;
            case PlayerClass.Shielder:
                damage.RemoveModifier(amount);
                break;
        }
    }

    public void RemoveIntelligence(int amount)
    {
        intelligence.RemoveModifier(amount);
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                break;
            case PlayerClass.Blader:
                break;
            case PlayerClass.Archer:
                break;
            case PlayerClass.Wizard:
                damage.RemoveModifier(amount);
                break;
            case PlayerClass.Shielder:
                break;
        }
    }

    public void RemoveDexterity(int amount)
    {
        dexterity.RemoveModifier(amount);
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                defense.RemoveModifier(amount);
                break;
            case PlayerClass.Blader:
                damage.RemoveModifier(amount);
                break;
            case PlayerClass.Archer:
                damage.RemoveModifier(amount);
                break;
            case PlayerClass.Wizard:
                defense.RemoveModifier(amount);
                break;
            case PlayerClass.Shielder:
                defense.RemoveModifier(amount);
                break;
        }
    }
}