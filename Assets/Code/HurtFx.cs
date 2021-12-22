using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtFx : MonoBehaviour
{
    public Image image;
    public float duration = 1f;

    private bool animating;
    private float time = 0f;

    private float Alpha
    {
        get => image.color.a;
        set
        {
            Color c = image.color;
            c.a = value;
            image.color = c;
        }
    }

    public void Show()
    {
        animating = true;
        Alpha = 1f;
        image.gameObject.SetActive(true);
        time = 0f;
    }

    private void Update()
    {
        if (!animating)
            return;

        time += Time.deltaTime;
        float timeVal = time / duration;
        Alpha = 1f - timeVal;

        if (timeVal >= 1f)
        {
            animating = false;
            image.gameObject.SetActive(false);
        }
    }
}
