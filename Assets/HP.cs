using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour {
    public void HP_Change(int x) {
        RectTransform rt = GetComponent<RectTransform>();
        rt.localScale = new Vector3(x / 100f, 1f, 1f);
        Image img = GetComponent<Image>();
        if (img != null) {
            if (x >= 50) {
                img.color = Color.green;
            } else if (x < 50 && x > 25) {
                img.color = Color.yellow;
            } else {
                img.color = Color.red;
            }
        }
    }
}