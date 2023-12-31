using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmComponent : MonoBehaviour
{
    [SerializeField]
    [Min(1)]
    private float VisionRadius = 1;

    [SerializeField]
    [Min(0)]
    private float SeparationRadius = 0.5f;

    [SerializeField]
    [Min(0)]
    private float HeadingWeight = 1;

    [SerializeField]
    [Min(0)]
    private float CenterWeight = 1;

    [SerializeField]
    [Min(0)]
    private float SeparationWeight = 1;

    [SerializeField]
    [Min(0)]
    private float TargetWeight = 1;

    [SerializeField]
    [Min(1)]
    private float Speed = 10;

    [SerializeField]
    [Min(0)]
    private int Damage = 1;

    [SerializeField]
    [Min(0)]
    private int Knockback = 5;

    [SerializeField]
    private AudioSource DamageSound;

    private static List<SwarmComponent> SwarmElements = new List<SwarmComponent>();
    private PlayerControler Character;

    private Vector3 SwarmCenter;
    private Vector3 SwarmHeading;
    private Vector3 Separation;
    private int NumberOfVisibleEnnemy;

    private Vector3 Velocity;

    private HealthComponent Health;

    // Start is called before the first frame update
    void Start()
    {
        SwarmElements.Add(this);
        Character = FindObjectOfType<PlayerControler>();
        Velocity = Speed * transform.forward;
        Health = gameObject.GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {        
        GetSwarmInformation();

        Vector3 direction = Vector3.zero;

        SteerWithSwarmInformation(ref direction);
        SteerTowardTarget(ref direction);
        AvoidObstacle(ref direction);

        if(Health.IsAlive) UpdatePosition(direction);
    }

    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * Speed - Velocity;
        return v;
    }

    private void GetSwarmInformation()
    {
        SwarmCenter = Vector3.zero;
        SwarmHeading = Vector3.zero;
        Separation = Vector3.zero;
        NumberOfVisibleEnnemy = 0;

        foreach (SwarmComponent swarmEnnemy in SwarmElements)
        {
            Vector3 separationBetween = swarmEnnemy.transform.position - this.transform.position;
            float distance = separationBetween.magnitude;

            if (distance < VisionRadius && !Mathf.Approximately(distance, 0.0f))
            {
                SwarmCenter += swarmEnnemy.transform.position;
                SwarmHeading += swarmEnnemy.transform.forward;

                if (distance < SeparationRadius)
                {
                    Separation -= separationBetween / distance;
                }

                NumberOfVisibleEnnemy++;
            }
        }

        SwarmCenter /= NumberOfVisibleEnnemy;
        SwarmHeading /= NumberOfVisibleEnnemy;
        Separation /= NumberOfVisibleEnnemy;
    }

    private void SteerWithSwarmInformation(ref Vector3 direction)
    {
        if (NumberOfVisibleEnnemy != 0)
        {
            direction = SteerTowards(SwarmCenter) * CenterWeight + SteerTowards(SwarmHeading) * HeadingWeight + SteerTowards(Separation) * SeparationWeight;
        }
    }

    private void SteerTowardTarget(ref Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (Character.transform.position - transform.position).normalized, out hit))
        {
            if (hit.collider.tag == "Character" && hit.collider.enabled)
            {
                direction += SteerTowards(Character.transform.position - transform.position) * TargetWeight;
            }
        }
    }

    private void AvoidObstacle(ref Vector3 direction)
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        if (Physics.Raycast(ray, out hit, VisionRadius / 2) && (hit.collider.tag != "Enemy") && (hit.collider.tag != "Character"))
        {
            direction += (10.0f / hit.distance) * SteerTowards(FindFreeDirection());
        }
    }

    private Vector3 FindFreeDirection()
    {
        const int angleStep = 5;
        for(int angle = 0; angle < 360; angle += angleStep)
        {
            Vector3 direction = Quaternion.Euler(angle, 0, 0) * transform.forward;
            RaycastHit hit;
            if(!Physics.Raycast(transform.position, direction, out hit, VisionRadius / 2))
            {
                return direction;
            }
        }

        return Vector3.zero;
    }

    private void UpdatePosition(Vector3 direction) 
    {
        direction.x = 0;
        Velocity += direction * Time.deltaTime;

        float speed = Velocity.magnitude;
        Vector3 dir = Velocity / speed;
        speed = Mathf.Clamp(speed, 2, Speed);
        Velocity = dir * speed;

        transform.position += Velocity * Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, dir, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Character" && Health.IsAlive)
        {
            collision.gameObject.GetComponent<HealthComponent>().TakeDamage(Damage);
            DamageSound?.Play();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Character" && Health.IsAlive)
        {
            Vector3 direction = (collision.gameObject.transform.position - gameObject.transform.position);
            direction.Normalize();
            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * Knockback, ForceMode.Impulse);
        }
    }

    public void OnDeath()
    {
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        SwarmElements.Remove(this);
    }
}
