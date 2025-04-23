using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupAutoControl : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (canvasGroup.alpha >= 0.2f)
        {
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
        }
    }
}
