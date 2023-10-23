using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarViewComponent : MonoBehaviour
{
    [SerializeField] RectTransform container;
    [SerializeField] RectTransform bar;
    [SerializeField] RectTransform intermediateBar;

    [Min(0.01f)]
    [SerializeField] float speed;

    IEnumerator coroutine;

    float percentValue;
    public float PercentValue
    {
        set
        {
            if (value == percentValue) return;
            if (value < 0.0f) value = 0.0f;
            if (value > 1.0f) value = 1.0f;

            if (coroutine is not null) StopCoroutine(coroutine);
            coroutine = InterpolateIntermediateBar(value - percentValue);

            percentValue = value;

            SetBarPercent(value);
            StartCoroutine(coroutine);
        }
        get => percentValue;
    }

    private void Awake()
    {
        SetBarPercent(0);
        intermediateBar?.gameObject.SetActive(false);
    }

    void SetBarPercent(float percent) => bar?.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, container.rect.width * percent);
 
    IEnumerator InterpolateIntermediateBar(float range)
    {
        intermediateBar.gameObject.SetActive(true);

        

        while (Mathf.Abs(range) > 0)
        {
            if (range > 0)
            {
                range -= speed * Time.deltaTime;
                if (range < 0) range = 0;

                intermediateBar.anchoredPosition = new Vector2(container.rect.width * (PercentValue - range), 0);
            }
            else
            {
                range += speed * Time.deltaTime;
                if (range > 0) range = 0;

                intermediateBar.anchoredPosition = new Vector2(container.rect.width * PercentValue, 0);
            }

            intermediateBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, container.rect.width * Mathf.Abs(range));

            yield return 0;
        }

        intermediateBar.gameObject.SetActive(false);
        coroutine = null;
    }
}
