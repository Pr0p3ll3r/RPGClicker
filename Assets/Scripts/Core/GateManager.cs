using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GateManager : MonoBehaviour
{
    [SerializeField] private Location heavenGate;
    [SerializeField] private Location hellGate;
    [SerializeField] private float gateTimer;
    [SerializeField] private TextMeshProUGUI gateTimerText;
    [SerializeField] private Button heavenGateButton;
    [SerializeField] private Button hellGateButton;

    private float currentGateTimer;
    public static bool inGate;
    private PlayerInventory Inventory => PlayerInventory.Instance;
    private GameManager GameManager => GameManager.Instance;

    private void Start()
    {
        heavenGateButton.onClick.AddListener(delegate { StartGate(heavenGate); });
        hellGateButton.onClick.AddListener(delegate { StartGate(hellGate); });
    }

    private void Update()
    {
        if (!inGate) return;

        if (currentGateTimer > 0)
        {
            currentGateTimer -= Time.deltaTime;
            gateTimerText.text = $"Remaining time {Mathf.FloorToInt(currentGateTimer + 1):00}";
        }
        else
        {
            currentGateTimer = gateTimer;
            gateTimerText.text = "";
            GameManager.StopGate();
            inGate = false;
        }
    }

    private void StartGate(Location location)
    {
        if (!Inventory.HaveEnoughGold(location.price))
        {
            GameManager.ShowText("Not enough gold!", Color.red);
        }
        else
        {
            Inventory.ChangeGoldAmount(-location.price);
            inGate = true;
            currentGateTimer = gateTimer;
            GameManager.ChangeLocation(location);
        }
    }
}
