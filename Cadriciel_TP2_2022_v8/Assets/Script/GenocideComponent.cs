using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenocideComponent : MonoBehaviour
{
    UnityAction OnGenocideCompleted;

    int killCount;

    [SerializeField]
    List<HealthComponent> enemiesToDefeat;

    public bool HasBeenCleared { private set; get; }

    private void Start()
    {
        foreach (HealthComponent healthComponent in enemiesToDefeat) healthComponent.OnDeath.AddListener(OnEnemyDeath);
    }

    private void OnDestroy()
    {
        foreach (HealthComponent healthComponent in enemiesToDefeat) healthComponent.OnDeath.RemoveListener(OnEnemyDeath);
    }

    public void AddEnemyToDefeat(HealthComponent healthComponent)
    {
        if (HasBeenCleared) return;

        enemiesToDefeat.Add(healthComponent);
        healthComponent.OnDeath.AddListener(OnEnemyDeath);
    }

    void OnEnemyDeath()
    {
        if (HasBeenCleared || ++killCount <= enemiesToDefeat.Count) return;

        HasBeenCleared = true;

        foreach (HealthComponent healthComponent in enemiesToDefeat) healthComponent.OnDeath.RemoveListener(OnEnemyDeath);

        OnGenocideCompleted?.Invoke();
    }
}
