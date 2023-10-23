using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZoneComponent : MonoBehaviour
{
    public UnityAction<ZoneComponent> OnZoneEntered;
    public UnityAction<ZoneComponent> OnZoneLeft;

    [SerializeField] new string name;
    public string Name { get => name; }

    [SerializeField] string description;
    public string Description { get => description; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        OnZoneEntered?.Invoke(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        OnZoneLeft?.Invoke(this);
    }
}
