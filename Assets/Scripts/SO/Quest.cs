using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest")]
public class Quest : ScriptableObject
{
    public int ID;
    public EnemyData goal;
    public int amount;
    public bool randomReward;
    public ItemStack[] rewardWarrior;
    public ItemStack[] rewardBlader;
    public ItemStack[] rewardArcher;
    public ItemStack[] rewardWizard;
    public ItemStack[] rewardShielder;
}
