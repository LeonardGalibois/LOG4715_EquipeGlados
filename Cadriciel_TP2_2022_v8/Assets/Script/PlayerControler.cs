﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    // Déclaration des constantes
    private static readonly Vector3 FlipRotation = new Vector3(0, 180, 0);
    private static readonly Vector3 CameraPosition = new Vector3(4, 1, 0);
    private static readonly Vector3 InverseCameraPosition = new Vector3(-4, 1, 0);

    // Déclaration des variables
    bool _Grounded { get; set; }
    bool _Flipped { get; set; }
    Animator _Anim { get; set; }
    Rigidbody _Rb { get; set; }
    Camera _MainCamera { get; set; }
    bool canDash = true;


    // Valeurs exposées
    [SerializeField]
    float MoveSpeed = 5.0f;

    [SerializeField]
    float JumpForce = 10f;

    [SerializeField]
    float DashForce = 10f;

    [SerializeField]
    float dashCooldown = 1f;

    [SerializeField]
    float jumpIncrease = 2f;

    [SerializeField]
    float jumpDecrease = 2f;

    [SerializeField]
    LayerMask WhatIsGround;
    
    [SerializeField]
    TrailRenderer DashTrail;

    [SerializeField]
    GameManager GM;

    // Awake se produit avait le Start. Il peut être bien de régler les références dans cette section.
    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        _MainCamera = Camera.main;
    }

    // Utile pour régler des valeurs aux objets
    void Start()
    {
        _Grounded = false;
        _Flipped = false;
    }

    // Vérifie les entrées de commandes du joueur
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * MoveSpeed;
        HorizontalMove(horizontal);
        FlipCharacter(horizontal);
        CheckJump();
        CheckDash();
    }

    // Gère le mouvement horizontal
    void HorizontalMove(float horizontal)
    {
        _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, horizontal);
        _Anim.SetFloat("MoveSpeed", Mathf.Abs(horizontal));
    }

    // Gère le saut du personnage, ainsi que son animation de saut
    void CheckJump()
    {
        if (_Grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _Rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
                _Grounded = false;
                _Anim.SetBool("Grounded", false);
                _Anim.SetBool("Jump", true);
            }
        }
    }

    // Gère la ruée du personnage, ainsi que son animation de ruée


    void CheckDash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            StartCoroutine(Dash());
        }
    }
    IEnumerator Dash()
    {
            if (canDash)
            {
                canDash = false;
                if (_Flipped)
                {
                    _Rb.AddForce(new Vector3(0, 0, transform.localScale.z * -DashForce), ForceMode.VelocityChange);
                }
                else
                {
                    _Rb.AddForce(new Vector3(0, 0, transform.localScale.z * DashForce), ForceMode.VelocityChange);
            }
                DashTrail.emitting = true;
                yield return new WaitForSeconds(dashCooldown);
                DashTrail.emitting = false;
                //yield return new WaitForSeconds(dashCooldown);
                canDash = true;
            }
    }

    // Gère l'orientation du joueur et les ajustements de la camera
    void FlipCharacter(float horizontal)
    {
        if (horizontal < 0 && !_Flipped)
        {
            _Flipped = true;
            transform.Rotate(FlipRotation);
            _MainCamera.transform.Rotate(-FlipRotation);
            _MainCamera.transform.localPosition = InverseCameraPosition;
        }
        else if (horizontal > 0 && _Flipped)
        {
            _Flipped = false;
            transform.Rotate(-FlipRotation);
            _MainCamera.transform.Rotate(FlipRotation);
            _MainCamera.transform.localPosition = CameraPosition;
        }
    }

    public bool isFlipped()
    {
        return _Flipped;
    }

    // Collision avec le sol
    void OnCollisionEnter(Collision coll)
    {
        specialCollsion(coll);

        // On s'assure de bien être en contact avec le sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) == 0)
            return;

        // Évite une collision avec le plafond
        if (coll.relativeVelocity.y > 0)
        {
            _Grounded = true;
            _Anim.SetBool("Grounded", _Grounded);
        }
    }

    void specialCollsion(Collision coll)
    {
        if (coll.gameObject.tag == "Untagged")
        {
            return;
        }
        if (coll.gameObject.tag.Contains("Key"))
        {
            GM?.ObtainKey(coll.gameObject.tag);
        }
        if (coll.gameObject.tag.Contains("Door"))
        {
            GM?.openDoor(coll.gameObject);
        }
        if (coll.gameObject.tag.Contains("Stat"))
        {
            if (coll.gameObject.name.Contains("Speed"))
            {
                
                if (coll.gameObject.name.Contains("Decrease"))
                {
                    MoveSpeed -= 1;
                }
                if (coll.gameObject.name.Contains("Increase"))
                {
                    MoveSpeed += 1;
                }
                GM?.BoostManager("speed", MoveSpeed != 5.0f, MoveSpeed > 5.0f);
                StartCoroutine(waitRespawn(coll));
            }
            if (coll.gameObject.name.Contains("Jump"))
            {
                
                if (coll.gameObject.name.Contains("Decrease"))
                {
                    JumpForce -= jumpIncrease;
                }
                if (coll.gameObject.name.Contains("Increase"))
                {
                    JumpForce += jumpDecrease;
                }
                GM?.BoostManager("jump", JumpForce != 10f, JumpForce > 10f);
                StartCoroutine(waitRespawn(coll));
            }

        }
    }
    IEnumerator waitRespawn(Collision coll)
    {
        coll.gameObject.SetActive(false);

        yield return new WaitForSeconds(1);

        coll.gameObject.SetActive(true);
    }
}