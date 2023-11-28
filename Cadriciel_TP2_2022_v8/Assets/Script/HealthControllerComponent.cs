using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControllerComponent : MonoBehaviour
{
    [SerializeField]
    HealthComponent healthComponent;

    [SerializeField]
    BarViewComponent healthBar;

    private void OnEnable()
    {
        healthComponent.OnHealthUpdate.AddListener(UpdateHealthBar);

        healthBar.SetPercentValueInstantly((float)healthComponent.Health / healthComponent.MaximumHealth);
    }

    private void OnDisable()
    {
        healthComponent.OnHealthUpdate.RemoveListener(UpdateHealthBar);
    }

    void UpdateHealthBar(float value)
    {
        healthBar.PercentValue = (float)healthComponent.Health / healthComponent.MaximumHealth;
    }
}
