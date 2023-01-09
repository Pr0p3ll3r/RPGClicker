using TMPro;
using UnityEngine;

public class LocationInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lvlMin;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI locationName;

    public void SetUp(Location l)
    {
        lvlMin.text = l.lvlMin.ToString();
        gold.text = l.price.ToString();
        locationName.text = l.locationName;
    }
}
