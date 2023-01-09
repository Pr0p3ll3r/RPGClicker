using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStatInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image image;

    public void SetUp(string title, string value)
    {
        titleText.text = title;
        valueText.text = value;
    }

    public void SetUp(string title, string value, Item item)
    {
        titleText.text = title;
        valueText.text = value;
        image.sprite = item.icon;
        image.gameObject.SetActive(true);
    }

    public void SetColor(Color color)
    {
        valueText.color = color;
    }
}
