using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Test Ability", fileName = "TestAbility")]
public class TestAbility : Ability
{
    public override void Activate(AbilitiesManagerComponent caster)
    {
        Debug.Log($"Test Ability \"{DisplayName}\" has just been activated!");
    }
}
