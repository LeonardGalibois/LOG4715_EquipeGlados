using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheatControllerComponent : MonoBehaviour
{
    [SerializeField]
    OverheatComponent overheatComponent;

    [SerializeField]
    BarViewComponent barViewComponent;

    [SerializeField]
    GameObject overheatIndicator;

    void Start()
    {
        barViewComponent.PercentValue = (float)overheatComponent.Heat / overheatComponent.MaxHeat;
        overheatComponent.OnHeatUpdated.AddListener(UpdateOverheatBar);
        overheatComponent.OnOverheated.AddListener(OnOverheated);
        overheatComponent.OnRestored.AddListener(OnRestored);
    }

    void UpdateOverheatBar(int value)
    {
        barViewComponent.PercentValue = (float)value / overheatComponent.MaxHeat;
    }

    void OnOverheated()
    {
        overheatIndicator?.SetActive(true);
    }

    void OnRestored()
    {
        overheatIndicator?.SetActive(false);
    }
}
