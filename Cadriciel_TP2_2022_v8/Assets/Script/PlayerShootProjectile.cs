using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectile : MonoBehaviour
{

    [SerializeField] Vector3 offset;
    Animator _Anim { get; set; }
    [SerializeField]
    GameObject BulletPrefab;
    [SerializeField]
    float Cooldown = 0.25f;
    [SerializeField]
    int HeatCost = 5;
    [SerializeField]
    OverheatComponent overheatComponent;

    float nextAvailableTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        _Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !IsOnCooldown() && !overheatComponent.IsOverheated)
        {
            overheatComponent.AddHeat(HeatCost);
            nextAvailableTime = Time.time + Cooldown;
            _Anim.SetTrigger("Shoot");

            GameObject bullet = Instantiate(BulletPrefab, transform.position + transform.TransformDirection(offset), Quaternion.identity);

            Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(bullet.transform.position);
            direction = (Camera.main.transform.right * direction.x + Camera.main.transform.up * direction.y).normalized;

            bullet.transform.LookAt(bullet.transform.position + direction);
        }
    }

    bool IsOnCooldown() => Time.time < nextAvailableTime;
}
