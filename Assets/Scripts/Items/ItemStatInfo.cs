using TMPro;
using UnityEngine;

public class ItemStatInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI valueText;

    public void SetUp(string title, string value)
    {
        titleText.text = title;
        valueText.text = value;
    }

    public void SetColor(Color color)
    {
        valueText.color = color;
    }
}
