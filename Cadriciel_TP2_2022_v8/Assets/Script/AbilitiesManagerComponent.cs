using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OverheatComponent))]
public class AbilitiesManagerComponent : MonoBehaviour
{
    OverheatComponent overheatComponent;
    float nextAvailableCastTime;
    Dictionary<Ability, float> abilityCooldownTimes;

    void Start()
    {
        overheatComponent = GetComponent<OverheatComponent>();
        abilityCooldownTimes = new Dictionary<Ability, float>();
    }

    bool IsAbilityOnCooldown(Ability ability)
    {
        float cooldownTime;

        return abilityCooldownTimes.TryGetValue(ability, out cooldownTime) && Time.time < cooldownTime;
    }

    bool CanCastAbility()
    {
        return Time.time >= nextAvailableCastTime;
    }

    public bool CanActivateAbility(Ability ability)
    {
        return !overheatComponent.IsOverheated && CanCastAbility() && !IsAbilityOnCooldown(ability);
    }

    public void ActivateAbility(Ability ability)
    {
        if (!CanActivateAbility(ability)) return;

        nextAvailableCastTime = Time.time + ability.CastTime;
        abilityCooldownTimes[ability] = Time.time + ability.Cooldown;

        overheatComponent.AddHeat(ability.HeatCost);
        
        ability.Activate(this);
    }
}