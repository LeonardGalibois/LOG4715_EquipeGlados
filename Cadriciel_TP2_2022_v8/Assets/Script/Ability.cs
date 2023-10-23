using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [Header("Information")]
    [SerializeField]
    string displayName;

    [SerializeField]
    [TextArea(5,15)]
    string description;

    [Header("Properties")]
    [SerializeField]
    [Min(0)]
    float cooldown;

    [SerializeField]
    [Min(0)]
    float castTime;

    [SerializeField]
    [Min(0)]
    int heatCost;

    public string DisplayName { get => displayName; }
    public string Description { get => description; }
    public float Cooldown { get => cooldown; }
    public float CastTime { get => castTime; }
    public int HeatCost { get => heatCost; }

    public abstract void Activate(AbilitiesManagerComponent caster);
}
