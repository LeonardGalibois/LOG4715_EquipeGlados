using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectile : MonoBehaviour
{

    Vector3 Zoffset = new Vector3(0, 0, 0.3f);
    Vector3 Yoffset = new Vector3(0, 0.55f, 0);
    bool CanShoot = true;
    Animator _Anim { get; set; }
    [SerializeField]
    GameObject BulletPrefab;
    [SerializeField]
    float BulletCooldown = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        _Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanShoot && Input.GetButtonDown("Fire1"))
        {
            _Anim.SetTrigger("Shoot");
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
