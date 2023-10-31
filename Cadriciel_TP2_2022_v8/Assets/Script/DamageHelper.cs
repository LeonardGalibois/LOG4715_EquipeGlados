using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHelper : MonoBehaviour
{
    [SerializeField]
    [Min(1)]
    private int DamageAmount = 2;

    [SerializeField]
    [Min(1)]
    private int HealAmount = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DamageFromClick();
        HealFromClick();
    }

    private void DamageFromClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                HealthComponent health = hit.collider.GetComponent<HealthComponent>()
                                ?? hit.collider.GetComponentInParent<HealthComponent>()
                                ?? hit.collider.GetComponentInChildren<HealthComponent>();

                health?.TakeDamage(DamageAmount);
            }
        }
    }

    private void HealFromClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                HealthComponent health = hit.collider.GetComponent<HealthComponent>() 
                                ?? hit.collider.GetComponentInParent<HealthComponent>() 
                                ?? hit.collider.GetComponentInChildren<HealthComponent>();


                health?.Heal(HealAmount);
            }

        }
    }
}
