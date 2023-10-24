using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverheatComponent : MonoBehaviour
{
    float nextCooldownTime = 0;

    [SerializeField] [Min(0)]
    int maxHeat;

    [Header("Cool Down")]
    
    [SerializeField] [Min(0)]
    float delayBeforeCoolingDown;

    [SerializeField] [Min(0)]
    float cooldownSpeed;

    [Header("Overheat")]

    [SerializeField] [Min(0)]
    float overheatStallDuration = 3.0f;

    [Header("Events")]

    public UnityEvent<int> OnHeatUpdated;
    public UnityEvent OnOverheated;
    public UnityEvent OnRestored;

    int heat;
    public int Heat
    {
        private set
        {
            if (value == heat) return;

            if (value >= MaxHeat)
            {
                IsOverheated = true;
                heat = MaxHeat;
                OnOverheated?.Invoke();
            }
            else heat = value >= 0 ? value : 0;

            OnHeatUpdated?.Invoke(heat);
        }
        get => heat;
    }

    public int MaxHeat { get => maxHeat; }

    public bool IsOverheated { private set; get; }

    private void Start()
    {
        Heat = 0;
    }

    private void Update()
    {
        if (Time.time < nextCooldownTime) return;

        Heat--;

        nextCooldownTime = Time.time + 1.0f / cooldownSpeed;

        if (IsOverheated && Heat == 0) Restore();
    }

    public void AddHeat(int heat)
    {
        if (IsOverheated) return; 
            
        Heat += heat;

        nextCooldownTime = Time.time + (IsOverheated ? overheatStallDuration : delayBeforeCoolingDown);
    }

    public void Restore()
    {
        Heat = 0;
        IsOverheated = false;
        OnRestored?.Invoke();
    }
}
