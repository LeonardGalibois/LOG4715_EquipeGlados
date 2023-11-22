using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float BulletSpeed = 10f;
    [SerializeField]
    private float BulletLifeTime = 1.5f;
    [SerializeField]
    private int DamageAmout = 1;
    // Start is called before the first frame update
    void Start()
    {
        if(((PlayerControler)FindObjectOfType(typeof(PlayerControler))).isFlipped())
        {
            this.transform.Rotate(0, 180, 0);
        }
            this.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
        StartCoroutine(TimeOutBullet());
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
        }
        if(other.gameObject.tag != "Character")
        {
            Destroy(this.gameObject.transform.parent.gameObject);
            Destroy(gameObject);
        }
    }

    IEnumerator TimeOutBullet()
    {
        yield return new WaitForSeconds(BulletLifeTime);
        Destroy(this.gameObject.transform.parent.gameObject);
        Destroy(gameObject);
    }
}
