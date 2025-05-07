using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverImageChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite normalSprite;
    public Sprite hoverSprite;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        if (image != null && normalSprite != null)
        {
            image.sprite = normalSprite; // 起始狀態用原圖
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null && hoverSprite != null)
        {
            image.sprite = hoverSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image != null && normalSprite != null)
        {
            image.sprite = normalSprite;
        }
    }
}
