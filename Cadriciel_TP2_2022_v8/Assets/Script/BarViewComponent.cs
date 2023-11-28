using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarViewComponent : MonoBehaviour
{
    [SerializeField] RectTransform container;
    [SerializeField] RectTransform bar;
    [SerializeField] RectTransform increaseBar;
    [SerializeField] RectTransform decreaseBar;

    [Min(0.01f)]
    [SerializeField] float speed;

    float interpolationRange = 0;

    float percentValue;
    public float PercentValue
    {
        set
        {
            if (value == percentValue) return;
            if (value < 0.0f) value = 0.0f;
            if (value > 1.0f) value = 1.0f;

            interpolationRange = value - percentValue;

            percentValue = value;

            SetBarPercent(percentValue);
        }
        get => percentValue;
    }

    private void Awake()
    {
        increaseBar?.gameObject.SetActive(false);
        decreaseBar?.gameObject.SetActive(false);

        SetPercentValueInstantly(0);
        bar?.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.0f);
    }

    private void Update()
    {
        increaseBar?.gameObject.SetActive(interpolationRange > 0);
        decreaseBar?.gameObject.SetActive(interpolationRange < 0);

        if (interpolationRange == 0) return;

        RectTransform barToUse = interpolationRange > 0 ? increaseBar : decreaseBar;

        if (interpolationRange > 0)
        {
            interpolationRange -= speed * Time.deltaTime;
            if (interpolationRange < 0) interpolationRange = 0;

            barToUse.anchoredPosition = new Vector2(container.rect.width * (PercentValue - interpolationRange), 0);
        }
        else
        {
            interpolationRange += speed * Time.deltaTime;
            if (interpolationRange > 0) interpolationRange = 0;

            barToUse.anchoredPosition = new Vector2(container.rect.width * PercentValue, 0);
        }

        barToUse.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, container.rect.width * Mathf.Abs(interpolationRange));
    }

    public void SetPercentValueInstantly(float value)
    {
        PercentValue = value;
        Debug.Log(value);
        interpolationRange = 0;
    }

    void SetBarPercent(float percent) => bar?.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, container.rect.width * percent);
}
