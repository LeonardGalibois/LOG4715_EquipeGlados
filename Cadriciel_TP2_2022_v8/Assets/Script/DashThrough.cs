using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class DashThrough : MonoBehaviour
{
    Collider m_Collider;
    Rigidbody m_Rigidbody;
    PlayerControler m_PlayerController;
    Animator m_Animator;
    float m_nextAvailableTime;
    bool m_dashing;

    [SerializeField] string layerNameToDashThrough;
    [SerializeField] float cooldown = 1f;
    [SerializeField] float distance = 3f;
    [SerializeField] float duration = .5f;
    [SerializeField] int damage = 25;
    [SerializeField] int heatCost = 10;
    [SerializeField] TrailRenderer dashTrail;
    [SerializeField] OverheatComponent overheatComponent;


    // Start is called before the first frame update
    void Start()
    {
        m_Collider = GetComponentInChildren<Collider>();
        m_Rigidbody = GetComponentInChildren<Rigidbody>();
        m_PlayerController = GetComponentInChildren<PlayerControler>();
        m_Animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Dash") && !IsOnCooldown() && !overheatComponent.IsOverheated)
        {
            m_nextAvailableTime = Time.time + duration + cooldown;
            overheatComponent.AddHeat(heatCost);

            StartCoroutine(Dash());
        }
    }

    bool IsOnCooldown()
    {
        return Time.time < m_nextAvailableTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_dashing) return;

        HealthComponent health = other.gameObject.GetComponentInParent<HealthComponent>() ?? other.gameObject.GetComponentInChildren<HealthComponent>();

        if (health != null) health.TakeDamage(damage);
    }

    IEnumerator Dash()
    {
        float elapsedTime = 0f;
        float speed = distance / duration;
        Vector3 direction = transform.forward;

        m_PlayerController.FlipCharacter(Vector3.Dot(Camera.main.transform.right, direction));

        if (dashTrail != null) dashTrail.emitting = true;
        if (m_PlayerController != null) m_PlayerController.IgnoreInput = true;
        Physics.IgnoreLayerCollision(m_Rigidbody.gameObject.layer, LayerMask.NameToLayer(layerNameToDashThrough), true);
        m_dashing = true;

        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.useGravity = false;
        m_Rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.useGravity = true;

        Physics.IgnoreLayerCollision(m_Rigidbody.gameObject.layer, LayerMask.NameToLayer(layerNameToDashThrough), false);
        if (dashTrail != null) dashTrail.emitting = false;
        if (m_PlayerController != null) m_PlayerController.IgnoreInput = false;
        m_dashing = false;
    }
}
