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
            other.gameObject.GetComponent<HealthComponent>().TakeDamage(1);
            other.transform.parent.gameObject.transform
                .GetChild(1).gameObject.transform
                .GetChild(0).gameObject.transform
                .GetChild(0).gameObject.GetComponent<BarViewComponent>().PercentValue -= 0.1f;
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
