[System.Serializable]
public class PlayerData
{
    public string nickname;
    public PlayerClass playerClass;
    public int level;
    public int exp;
    public int remainingPoints;
    public int rebirthLevel;
    public int rebirthRemainingPoints;

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
    public Stat goldBonus;
    public Stat expBonus;
    public Stat hpSteal;
    public Stat hpStealLimit;
    public Stat twoSlotDropBonus;
    public Stat maxPets;

    public int killedMonsters;
    public int completedDungeons;
    public int completedQuests;
    public int earnedAchievements;

    public int currentTowerLevel;

    public bool IsFullHP()
    {
        if (currentHealth >= health.GetValue())
            return true;
        return false; 
    }

    public PlayerData() 
    { 
        nickname = "BOB";
        playerClass = PlayerClass.Warrior;
        level = 1;
        exp = 0;
        rebirthLevel = 0;
        rebirthRemainingPoints = 0;
        remainingPoints = 0;
        maxPets = new Stat(1);
        currentTowerLevel = 0;
        damage = new Stat(0);
        defense = new Stat(0);
        health = new Stat(20);
        strength = new Stat(0);
        intelligence = new Stat(0);
        dexterity = new Stat(0);
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                AddStrength(false, 12);
                AddIntelligence(false, 4);
                AddDexterity(false, 4);
                break;
            case PlayerClass.Blader:
                AddStrength(false, 8);
                AddIntelligence(false, 3);
                AddDexterity(false, 8);
                break;
            case PlayerClass.Archer:
                AddStrength(false, 6);
                AddIntelligence(false, 1);
                AddDexterity(false, 10);
                break;
            case PlayerClass.Wizard:
                AddStrength(false, 1);
                AddIntelligence(false, 10);
                AddDexterity(false, 6);
                break;
            case PlayerClass.Shielder:
                AddStrength(false, 6);
                AddIntelligence(false, 2);
                AddDexterity(false, 10);
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
        killedMonsters = 0;
        completedQuests = 0;
        earnedAchievements = 0;
    }

    public PlayerData(string _nickname, PlayerClass _playerClass)
    {
        nickname = _nickname;
        playerClass = _playerClass;
        level = 1;
        exp = 0;
        rebirthLevel = 0;
        rebirthRemainingPoints = 0;
        remainingPoints = 0;
        maxPets = new Stat(1);
        currentTowerLevel = 0;
        damage = new Stat(0);
        defense = new Stat(0);
        health = new Stat(20);
        strength = new Stat(0);
        intelligence = new Stat(0);
        dexterity = new Stat(0);
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                AddStrength(false, 12);
                AddIntelligence(false, 4);
                AddDexterity(false, 4);
                break;
            case PlayerClass.Blader:
                AddStrength(false, 8);
                AddIntelligence(false, 3);
                AddDexterity(false, 8);
                break;
            case PlayerClass.Archer:
                AddStrength(false, 6);
                AddIntelligence(false, 1);
                AddDexterity(false, 10);
                break;
            case PlayerClass.Wizard:
                AddStrength(false, 1);
                AddIntelligence(false, 10);
                AddDexterity(false, 6);
                break;
            case PlayerClass.Shielder:
                AddStrength(false, 6);
                AddIntelligence(false, 2);
                AddDexterity(false, 10);
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
        killedMonsters = 0;
        completedQuests = 0;
        earnedAchievements = 0;
    }

    public PlayerData Reborn(PlayerData currentData)
    {
        nickname = currentData.nickname;
        playerClass = currentData.playerClass;
        level = 1;
        exp = 0;
        remainingPoints = 0;
        rebirthLevel = currentData.rebirthLevel;
        rebirthRemainingPoints = currentData.rebirthRemainingPoints;
        maxPets = new Stat(1);
        currentTowerLevel = 0;
        damage = new Stat(0);
        defense = new Stat(0);
        health = new Stat(20);
        strength = new Stat(0);
        intelligence = new Stat(0);
        dexterity = new Stat(0);
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                AddStrength(false, 12);
                AddIntelligence(false, 4);
                AddDexterity(false, 4);
                break;
            case PlayerClass.Blader:
                AddStrength(false, 8);
                AddIntelligence(false, 3);
                AddDexterity(false, 8);
                break;
            case PlayerClass.Archer:
                AddStrength(false, 6);
                AddIntelligence(false, 1);
                AddDexterity(false, 10);
                break;
            case PlayerClass.Wizard:
                AddStrength(false, 1);
                AddIntelligence(false, 10);
                AddDexterity(false, 6);
                break;
            case PlayerClass.Shielder:
                AddStrength(false, 6);
                AddIntelligence(false, 2);
                AddDexterity(false, 10);
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
        killedMonsters = currentData.killedMonsters;
        completedQuests = 0;
        earnedAchievements = currentData.earnedAchievements;
        return this;
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
            case Stats.CriticalRatePercent:
                criticalRate.AddModifier(amount);
                break;
            case Stats.MaxCriticalRatePercent:
                maxCriticalRate.AddModifier(amount);
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
            case Stats.MaxPets:
                maxPets.AddModifier(amount);
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
            case Stats.CriticalRatePercent:
                criticalRate.RemoveModifier(amount);
                break;
            case Stats.MaxCriticalRatePercent:
                maxCriticalRate.RemoveModifier(amount);
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
            case Stats.MaxPets:
                maxPets.RemoveModifier(amount);
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
                break;
            case PlayerClass.Blader:
                defense.AddModifier(amount);
                break;
            case PlayerClass.Archer:
                defense.AddModifier(amount);
                break;
            case PlayerClass.Wizard:
                health.AddModifier(amount * 2);
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
                health.AddModifier(amount * 2);
                break;
            case PlayerClass.Blader:
                health.AddModifier(amount * 2);
                break;
            case PlayerClass.Archer:
                health.AddModifier(amount * 2);
                break;
            case PlayerClass.Wizard:
                damage.AddModifier(amount);
                break;
            case PlayerClass.Shielder:
                health.AddModifier(amount * 2); 
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
                break;
            case PlayerClass.Blader:
                defense.RemoveModifier(amount);
                break;
            case PlayerClass.Archer:
                defense.RemoveModifier(amount);
                break;
            case PlayerClass.Wizard:
                health.RemoveModifier(amount * 2);
                currentHealth -= amount * 2;
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
                health.RemoveModifier(amount * 2);
                currentHealth -= amount * 2;
                break;
            case PlayerClass.Blader:
                health.RemoveModifier(amount * 2);
                currentHealth -= amount * 2;
                break;
            case PlayerClass.Archer:
                health.RemoveModifier(amount * 2);
                currentHealth -= amount * 2;
                break;
            case PlayerClass.Wizard:
                damage.RemoveModifier(amount);
                break;
            case PlayerClass.Shielder:
                health.RemoveModifier(amount * 2);
                currentHealth -= amount * 2;
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