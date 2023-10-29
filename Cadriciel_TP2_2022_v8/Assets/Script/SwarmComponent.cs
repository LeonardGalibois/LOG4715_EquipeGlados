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

    private static List<SwarmComponent> SwarmElements = new List<SwarmComponent>();
    private PlayerControler Character;

    private Vector3 SwarmCenter;
    private Vector3 SwarmHeading;
    private Vector3 Separation;
    private int NumberOfVisibleEnnemy;

    private Vector3 Velocity;

    // Start is called before the first frame update
    void Start()
    {
        SwarmElements.Add(this);
        Character = FindObjectOfType<PlayerControler>();
        Velocity = Speed * transform.forward;
    }

    // Update is called once per frame
    void Update()
    {        
        GetSwarmInformation();

        Vector3 direction = Vector3.zero;

        SteerWithSwarmInformation(ref direction);
        SteerTowardTarget(ref direction);
        AvoidObstacle(ref direction);

        UpdatePosition(direction);
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
            if (hit.collider.tag == "Character")
            {
                direction += SteerTowards(Character.transform.position - transform.position) * TargetWeight;
            }
        }
    }

    private void AvoidObstacle(ref Vector3 direction)
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        if (Physics.SphereCast(ray, 1.25f, out hit, VisionRadius, 1))
        {
            direction += SteerTowards(hit.normal);
        }
    }

    private void UpdatePosition(Vector3 direction) 
    {
        direction.x = 0;
        Velocity += direction * Time.deltaTime;

        float speed = Velocity.magnitude;
        Vector3 dir = Velocity / speed;
        speed = Mathf.Clamp(speed, 1, Speed);
        Velocity = dir * speed;

        transform.position += Velocity * Time.deltaTime;
        transform.forward = dir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Character")
        {
            collision.gameObject.GetComponent<HealthComponent>().TakeDamage(Damage);
        }
    }

    private void OnDisable()
    {
        SwarmElements.Remove(this);
    }
}
