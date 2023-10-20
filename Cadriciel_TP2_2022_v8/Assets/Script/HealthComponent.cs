using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [Min(1)]
    [SerializeField]
    private int MaximumHealth = 10;
    private int Health;

    private float HealthPercentage
    {
        get
        {
            return (float)Health / (float)MaximumHealth;
        }
    }

    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthUpdate;

    // Start is called before the first frame update
    void Start()
    {
        Health = MaximumHealth;
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        OnHealthUpdate.Invoke(HealthPercentage);

        if (Health <= 0)
        {
            OnDeath.Invoke();
        }
    }

    public void Heal(int amount)
    {
        Health += amount;
        if(Health > MaximumHealth)
        {
            Health = MaximumHealth;
        }

        OnHealthUpdate.Invoke(HealthPercentage);
    }
}
