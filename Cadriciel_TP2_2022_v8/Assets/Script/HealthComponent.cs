using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    //[SerializeField]
    //GameObject FloatingText;
    [SerializeField]
    [Min(1)]
    int maximumHealth = 10;
    public int MaximumHealth
    {
        private set
        {
            maximumHealth = value;

            OnHealthUpdate?.Invoke(HealthPercentage);
        }
        get => maximumHealth;
    }

    int health;
    public int Health
    {
        private set
        {
            if (value == health) return;

            if (value <= 0)
            {
                health = 0;
                OnDeath?.Invoke();
            }
            else health = value <= MaximumHealth ? value : MaximumHealth;

            OnHealthUpdate?.Invoke(HealthPercentage);
        }
        get => health;
    }

    public bool IsAlive { get => Health > 0; }

    private float HealthPercentage
    {
        get
        {
            return (float)Health / (float)MaximumHealth;
        }
    }

    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthUpdate;

    private void Awake() => Revive();

    public void TakeDamage(int amount)
    {
        if (IsAlive) Health -= amount;
    }

    public void Heal(int amount)
    {
        if (IsAlive) Health += amount;
    }

    public void Revive()
    {
        Health = MaximumHealth;
    }
}
