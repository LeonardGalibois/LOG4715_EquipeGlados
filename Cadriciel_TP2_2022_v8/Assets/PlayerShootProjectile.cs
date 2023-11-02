using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectile : MonoBehaviour
{

    Vector3 Zoffset = new Vector3(0, 0, 1);
    Vector3 Yoffset = new Vector3(0, 0.75f, 0);
    bool CanShoot = true;
    [SerializeField]
    GameObject BulletPrefab;
    [SerializeField]
    float BulletCooldown = 0.25f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CanShoot && Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(DisableShoot());
            if (((PlayerControler)FindObjectOfType(typeof(PlayerControler))).isFlipped())
            {
                Instantiate(BulletPrefab, this.transform.position - Zoffset + Yoffset , Quaternion.identity);
            }
            else
            {
                Instantiate(BulletPrefab, this.transform.position + Zoffset + Yoffset, Quaternion.identity);
            }
        }
    }
    IEnumerator DisableShoot()
    {
        CanShoot = false;
        yield return new WaitForSeconds(BulletCooldown);
        CanShoot = true;
    }
}
