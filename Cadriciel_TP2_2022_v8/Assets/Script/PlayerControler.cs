using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    // Déclaration des constantes
    private static readonly Vector3 FlipRotation = new Vector3(0, 180, 0);
    private static readonly Vector3 CameraPosition = new Vector3(4, 1, 0);
    private static readonly Vector3 InverseCameraPosition = new Vector3(-4, 1, 0);

    // Déclaration des variables
    Collider _collider;
    bool _Flipped { get; set; }
    Animator _Anim { get; set; }
    Rigidbody _Rb { get; set; }
    Camera _MainCamera { get; set; }


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
    float speedIncrease = 2f;

    [SerializeField]
    float speedDecrease = 2f;

    [SerializeField]
    LayerMask WhatIsGround;

    [SerializeField]
    float GroundCheckDistance;
    
    [SerializeField]
    TrailRenderer DashTrail;

    [SerializeField]
    GameManager GM;

    [SerializeField]
    AudioSource m_DeathSound;

    public bool IgnoreInput { set; get; }

    bool _isGrounded;
    public bool IsGrounded
    {
        set
        {
            if (value == _isGrounded) return;

            _isGrounded = value;
            _Anim.SetBool("Grounded", _isGrounded);
        }
        get => _isGrounded;
    }

    bool _isJumping;
    public bool IsJumping
    {
        set
        {
            if (value == _isJumping) return;

            _isJumping = value;
            _Anim.SetBool("Jump", _isJumping);
        }
        get => _isJumping;
    }

    // Awake se produit avait le Start. Il peut être bien de régler les références dans cette section.
    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();
        _MainCamera = Camera.main;
        GetComponentInChildren<HealthComponent>()?.OnDeath.AddListener(OnDeath);
    }

    // Utile pour régler des valeurs aux objets
    void Start()
    {
        _Flipped = false;
    }

    // Vérifie les entrées de commandes du joueur
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * MoveSpeed;
        if (!IgnoreInput)
        {
            HorizontalMove(horizontal);
            FlipCharacter(horizontal);
            CheckJump();
        }
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
        if (IsGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _Rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
                IsJumping = true;
                IsGrounded = false;
            }
        }
    }

    // Gère l'orientation du joueur et les ajustements de la camera
    public void FlipCharacter(float horizontal)
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

    void FixedUpdate()
    {
        Vector3 origin = _collider.bounds.center;
        float maxDistance = _collider.bounds.size.y / 2 + GroundCheckDistance;

        IsGrounded = Physics.Raycast(origin, Vector3.down, maxDistance, WhatIsGround);

        if (IsJumping && _Rb.velocity.y <= 0) IsJumping = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        specialCollsion(other);
    }

    public void OnDeath()
    {
        IgnoreInput = true;
        _Anim.SetTrigger("Die");
        m_DeathSound?.Play();
    }

    void specialCollsion(Collider coll)
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
                    MoveSpeed -= speedDecrease;
                }
                if (coll.gameObject.name.Contains("Increase"))
                {
                    MoveSpeed += speedIncrease;
                }
                GM?.BoostManager("speed", MoveSpeed != 5.0f, MoveSpeed > 5.0f);
                StartCoroutine(waitRespawn(coll, false));
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
                StartCoroutine(waitRespawn(coll, false));
            }

        }
    }
    IEnumerator waitRespawn(Collider coll, bool respawn)
    {
        coll.gameObject.SetActive(false);

        if(respawn)
        {
            yield return new WaitForSeconds(1);

            coll.gameObject.SetActive(true);
        }
    }
}