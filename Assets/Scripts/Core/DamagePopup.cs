using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour, IPooledObject
{
    private TextMeshProUGUI text;
    private float disappearTime;
    private Color textColor;
    private RectTransform rectTransform;

    public ObjectPooler Pool { get; set; }

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(int damage, bool crit)
    {
        text.SetText(damage.ToString());
        if(crit)
        {
            text.color = Color.red;
        }
        else
        {
            text.color = Color.white;
        }
        textColor = text.color;
        disappearTime = 1f;
    }

    private void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, 300f) * Time.deltaTime;

        disappearTime -= Time.deltaTime;
        if(disappearTime < 0)
        {
            textColor.a -= 3f * Time.deltaTime;
            text.color = textColor;
            if(textColor.a < 0)
            {
                Pool.ReturnToPool(gameObject);
            }
        }
    }
}
