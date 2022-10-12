using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshProUGUI text;
    private float disappearTime;
    private Color textColor;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Setup(int damage, bool crit)
    {
        text.SetText(damage.ToString());
        if(crit)
        {
            text.color = new Color32(255, 170, 20, 255);
        }
        else
        {
            text.color = new Color32(255, 170, 10, 255);
        }
        textColor = text.color;
        disappearTime = 1f;
    }

    private void Update()
    {
        transform.position += new Vector3(0, 500f) * Time.deltaTime;

        disappearTime -= Time.deltaTime;
        if(disappearTime < 0)
        {
            textColor.a -= 3f * Time.deltaTime;
            text.color = textColor;
            if(textColor.a < 0)
            {
                DamagePopupPooler.Instance.ReturnToPool(gameObject);
            }
        }
    }
}
