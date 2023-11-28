using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControllerComponent : MonoBehaviour
{
    [SerializeField]
    HealthComponent healthComponent;

    [SerializeField]
    BarViewComponent healthBar;

    private void Start()
    {
        Debug.Log($"Setting health instantly to {(float)healthComponent.Health / healthComponent.MaximumHealth} [{healthComponent.Health}/{healthComponent.MaximumHealth}]");
        healthBar.SetBarPercentInstantly((float)healthComponent.Health / healthComponent.MaximumHealth);
    }

    private void OnEnable()
    {
        healthComponent.OnHealthUpdate.AddListener(UpdateHealthBar);
    }

    private void OnDisable()
    {
        healthComponent.OnHealthUpdate.RemoveListener(UpdateHealthBar);
    }

    void UpdateHealthBar(float value)
    {
        healthBar.SetBarPercent((float)healthComponent.Health / healthComponent.MaximumHealth);
    }
}
