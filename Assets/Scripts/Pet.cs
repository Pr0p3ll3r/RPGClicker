using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Pet", menuName = "RPG/Character/Pet")]
public class Pet : ScriptableObject
{
    public int level = 1;
    public int exp;
    public int expToLvlUp = 10;
    public int index;
    public int cost;
    public ItemStat[] stats = new ItemStat[30];

    void CheckLvlUp()
    {
        expToLvlUp = 10 * level;

        while (exp >= expToLvlUp)
        {
            exp -= expToLvlUp;
            level++;
            Debug.Log(name + ": Level Up! Current level: " + level);
        }
    }

    public void AddExp(int amount)
    {
        if (level == 50) return;
        exp += amount;
        CheckLvlUp();
    }

    public Pet GetCopy()
    {
        return Instantiate(this);
    }
}
