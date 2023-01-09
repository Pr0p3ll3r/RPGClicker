using UnityEngine;
using UnityEngine.UI;

public class ClickSound : MonoBehaviour
{
    private void Start()
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>(true);

        foreach(Button b in buttons)
        {
            b.onClick.AddListener(delegate { SoundManager.Instance.PlayOneShot("Click"); });
        }
    }
}
