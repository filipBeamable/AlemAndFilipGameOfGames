using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public RectTransform rectTransform;
    public Slider slider;

    public void UpdateSlider(float sliderValue)
    {
        slider.value = sliderValue;
    }

    public void UpdatePosition(Vector3 position)
    {
        RectTransform CanvasRect = UIController.Instance.canvas.GetComponent<RectTransform>();

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        rectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }
}
