using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private Image questGoal;
    [SerializeField] private TextMeshProUGUI questProgress;
    [SerializeField] private GameObject itemRewardPrefab;
    [SerializeField] private Transform questReward;
    [SerializeField] private Button getRewardButton;
    [SerializeField] private GameObject randomReward;

    private int currentQuestProgress;
    private Quest currentQuest;
    private Player Player => Player.Instance;
    private PlayerInventory Inventory => PlayerInventory.Instance;

    private void Start()
    {
        getRewardButton.interactable = false;
    }

    public void SetQuest(Quest quest)
    {
        currentQuestProgress = PlayerPrefs.GetInt("QuestProgress", 0);
        if (quest != null)
        {
            currentQuest = quest;
            questName.text = currentQuest.name;
            questGoal.sprite = currentQuest.goal.icon;
            questProgress.text = $"{currentQuestProgress}/{quest.amount}";
            if(currentQuest.randomReward)
                randomReward.SetActive(true);
            else
                randomReward.SetActive(false);
            switch (Player.data.playerClass)
            {
                case PlayerClass.Warrior:
                    foreach (ItemStack itemStack in quest.rewardWarrior)
                    {
                        GameObject reward = Instantiate(itemRewardPrefab, questReward);
                        reward.GetComponent<InventorySlot>().FillSlot(itemStack.item, itemStack.amount);
                    }
                    break;
                case PlayerClass.Blader:
                    foreach (ItemStack itemStack in quest.rewardBlader)
                    {
                        GameObject reward = Instantiate(itemRewardPrefab, questReward);
                        reward.GetComponent<InventorySlot>().FillSlot(itemStack.item, itemStack.amount);
                    }
                    break;
                case PlayerClass.Archer:
                    foreach (ItemStack itemStack in quest.rewardArcher)
                    {
                        GameObject reward = Instantiate(itemRewardPrefab, questReward);
                        reward.GetComponent<InventorySlot>().FillSlot(itemStack.item, itemStack.amount);
                    }
                    break;
                case PlayerClass.Wizard:
                    foreach (ItemStack itemStack in quest.rewardWizard)
                    {
                        GameObject reward = Instantiate(itemRewardPrefab, questReward);
                        reward.GetComponent<InventorySlot>().FillSlot(itemStack.item, itemStack.amount);
                    }
                    break;
                case PlayerClass.Shielder:
                    foreach (ItemStack itemStack in quest.rewardShielder)
                    {
                        GameObject reward = Instantiate(itemRewardPrefab, questReward);
                        reward.GetComponent<InventorySlot>().FillSlot(itemStack.item, itemStack.amount);
                    }
                    break;
            }
            getRewardButton.onClick.AddListener(delegate { GetRewards(); });
        }
        else
        {
            questName.text = "Quests completed!";
            questGoal.sprite = Database.data.emptySlot;
            questProgress.text = "";
            randomReward.SetActive(false);
        }
    }

    public void QuestProgress(EnemyData enemy)
    {
        if (currentQuest == null)
            return;

        if (enemy.enemyName != currentQuest.goal.enemyName)
            return;

        if(currentQuestProgress != currentQuest.amount)
        {
            currentQuestProgress++;
            questProgress.text = $"{currentQuestProgress}/{currentQuest.amount}";
            PlayerPrefs.SetInt("QuestProgress", currentQuestProgress);
        }
        else
        {
            getRewardButton.interactable = true;
            GameManager.Instance.ShowText("Quest completed!", Color.green);
        }          
    }

    private void GetRewards()
    {
        if (currentQuest.randomReward)
        {
            if (Inventory.IsFull())
                return;

            ItemStack reward = null;
            switch (Player.data.playerClass)
            {
                case PlayerClass.Warrior:
                    reward = LootTable.QuestRewardRandom(currentQuest.rewardWarrior);
                    Inventory.AddItem(reward.item, reward.amount);
                    break;
                case PlayerClass.Blader:
                    reward = LootTable.QuestRewardRandom(currentQuest.rewardBlader);
                    Inventory.AddItem(reward.item, reward.amount);
                    break;
                case PlayerClass.Archer:
                    reward = LootTable.QuestRewardRandom(currentQuest.rewardArcher);
                    Inventory.AddItem(reward.item, reward.amount);
                    break;
                case PlayerClass.Wizard:
                    reward = LootTable.QuestRewardRandom(currentQuest.rewardWizard);
                    break;
                case PlayerClass.Shielder:
                    reward = LootTable.QuestRewardRandom(currentQuest.rewardShielder);                 
                    break;
            }
            Inventory.AddItem(reward.item, reward.amount);
        }
        else
        {
            switch (Player.data.playerClass)
            {
                case PlayerClass.Warrior:
                    if (!Inventory.HaveEnoughSlots(currentQuest.rewardWarrior))
                        return;
                    foreach (ItemStack itemStack in LootTable.QuestReward(currentQuest.rewardWarrior))
                    {
                        Inventory.AddItem(itemStack.item, itemStack.amount);
                    }
                    break;
                case PlayerClass.Blader:
                    if (!Inventory.HaveEnoughSlots(currentQuest.rewardBlader))
                        return;
                    foreach (ItemStack itemStack in LootTable.QuestReward(currentQuest.rewardBlader))
                    {
                        Inventory.AddItem(itemStack.item, itemStack.amount);
                    }
                    break;
                case PlayerClass.Archer:
                    if (!Inventory.HaveEnoughSlots(currentQuest.rewardArcher))
                        return;
                    foreach (ItemStack itemStack in LootTable.QuestReward(currentQuest.rewardArcher))
                    {
                        Inventory.AddItem(itemStack.item, itemStack.amount);
                    }
                    break;
                case PlayerClass.Wizard:
                    if (!Inventory.HaveEnoughSlots(currentQuest.rewardWizard))
                        return;
                    foreach (ItemStack itemStack in LootTable.QuestReward(currentQuest.rewardWizard))
                    {
                        Inventory.AddItem(itemStack.item, itemStack.amount);
                    }
                    break;
                case PlayerClass.Shielder:
                    if (!Inventory.HaveEnoughSlots(currentQuest.rewardShielder))
                        return;
                    foreach (ItemStack itemStack in LootTable.QuestReward(currentQuest.rewardShielder))
                    {
                        Inventory.AddItem(itemStack.item, itemStack.amount);
                    }
                    break;
            }
        }

        getRewardButton.interactable = false;
        getRewardButton.onClick.RemoveAllListeners();
        foreach (Transform child in questReward)
        {
            Destroy(child.gameObject);
        }
        //Set next quest
        currentQuestProgress = 0;
        PlayerPrefs.SetInt("QuestProgress", currentQuestProgress);
        Player.data.completedQuests++;
        if (currentQuest.ID == Database.data.quests.Length - 1) //no more quests
            SetQuest(null);
        else
            SetQuest(Database.data.quests[currentQuest.ID + 1]);
    }
}
