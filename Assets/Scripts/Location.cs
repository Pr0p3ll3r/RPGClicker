using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "RPG/Location")]
public class Location : ScriptableObject
{
    public new string name;
    public int price = 1;
    public bool discovered = false;
    public bool unlocked = false;
    public bool bossDefeated = false;
    public EnemyData[] enemies;
    public EnemyData boss;
}