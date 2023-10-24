using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AbilitySlot
{
    public KeyCode key;
    public Ability ability;
}

public class AbilitiesInputComponent : MonoBehaviour
{
    [SerializeField]
    public AbilitiesManagerComponent abilitiesManagerComponent;

    [SerializeField]
    AbilitySlot[] abilitySlots;

    // Update is called once per frame
    void Update()
    {
        foreach(AbilitySlot slot in abilitySlots)
        {
            if (!Input.GetKeyDown(slot.key)) continue;

            abilitiesManagerComponent?.ActivateAbility(slot.ability);
        }
    }
}
