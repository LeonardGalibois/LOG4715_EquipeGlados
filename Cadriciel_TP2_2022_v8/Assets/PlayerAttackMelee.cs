using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackMelee : MonoBehaviour
{
    bool CanAttack = true;
    Vector3 Zoffset = new Vector3(0, 0, 0.3f);
    Vector3 Yoffset = new Vector3(0, 0.75f, 0);
    Animator _Anim { get; set; }
    [SerializeField]
    GameObject MeleeHitboxPrefab;
    [SerializeField]
    float AttackCooldown = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        _Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanAttack && Input.GetButtonDown("Fire2"))
        {
            _Anim.SetTrigger("Punch");
            StartCoroutine(DisableAttack());
            if (((PlayerControler)FindObjectOfType(typeof(PlayerControler))).isFlipped())
            {
                Instantiate(MeleeHitboxPrefab, this.transform.position - Zoffset + Yoffset, Quaternion.identity);
            }
            else
            {
                Instantiate(MeleeHitboxPrefab, this.transform.position + Zoffset + Yoffset, Quaternion.identity);
            }
        }
    }
    IEnumerator DisableAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }
}
