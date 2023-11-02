using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
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
            other.gameObject.GetComponent<HealthComponent>().TakeDamage(1);
            other.transform.parent.gameObject.transform
                .GetChild(1).gameObject.transform
                .GetChild(0).gameObject.transform
                .GetChild(0).gameObject.GetComponent<BarViewComponent>().PercentValue -= 0.1f;
            Destroy(this.gameObject);
        }
    }

    IEnumerator TimeOutHitbox()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
