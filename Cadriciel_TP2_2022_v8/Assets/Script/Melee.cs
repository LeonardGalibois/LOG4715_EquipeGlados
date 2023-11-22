using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField]
    private int DamageAmout = 1;
    // Start is called before the first frame update
    void Start()
    {
        if (((PlayerControler)FindObjectOfType(typeof(PlayerControler))).isFlipped())
        {
            this.transform.Rotate(0, 180, 0);
        }
        this.GetComponent<Rigidbody>().velocity = transform.forward * 1f;
        StartCoroutine(TimeOutHitbox());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HealthComponent health = other.gameObject.GetComponent<HealthComponent>()
                                ?? other.gameObject.GetComponentInParent<HealthComponent>()
                                ?? other.gameObject.GetComponentInChildren<HealthComponent>();

            health.TakeDamage(DamageAmout);
            Destroy(this.gameObject);
        }
    }

    IEnumerator TimeOutHitbox()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
