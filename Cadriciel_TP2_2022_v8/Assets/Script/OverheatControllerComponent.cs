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
        barViewComponent.SetPercentValueInstantly((float)overheatComponent.Heat / overheatComponent.MaxHeat);

        if (overheatComponent.IsOverheated) OnOverheated();
        else OnRestored();
    }

    private void OnEnable()
    {
        overheatComponent.OnHeatUpdated.AddListener(UpdateOverheatBar);
        overheatComponent.OnOverheated.AddListener(OnOverheated);
        overheatComponent.OnRestored.AddListener(OnRestored);
    }

    private void OnDisable()
    {
        overheatComponent.OnHeatUpdated.RemoveListener(UpdateOverheatBar);
        overheatComponent.OnOverheated.RemoveListener(OnOverheated);
        overheatComponent.OnRestored.RemoveListener(OnRestored);
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
