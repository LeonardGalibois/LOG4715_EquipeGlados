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
    private float Speed = 10;

    private SwarmManagerComponent Manager;
    private PlayerControler Character;

    private Vector3 Velocity;

    // Start is called before the first frame update
    void Start()
    {
        Manager = FindObjectOfType<SwarmManagerComponent>();
        Character = FindObjectOfType<PlayerControler>();
        Velocity = Speed * transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 swarmCenter = Vector3.zero;
        Vector3 swarmHeanding = Vector3.zero;
        Vector3 separation = Vector3.zero;
        int numberOfVisibleEnnemy = 0;
        
        foreach(SwarmComponent swarmEnnemy in Manager.SwarmEnnemies)
        {
            Vector3 separationBetween = swarmEnnemy.transform.position - this.transform.position;
            float distance = separationBetween.magnitude;

            if (distance < VisionRadius && !Mathf.Approximately(distance, 0.0f))
            {
                swarmCenter += swarmEnnemy.transform.position;
                swarmHeanding += swarmEnnemy.transform.forward;

                if(distance < SeparationRadius)
                {
                    separation -= separationBetween / distance;
                }

                numberOfVisibleEnnemy++;
            }
        }

        swarmCenter /= numberOfVisibleEnnemy;
        swarmHeanding /= numberOfVisibleEnnemy;
        separation /= numberOfVisibleEnnemy;

        Vector3 direction = Vector3.zero;
        if(numberOfVisibleEnnemy != 0)
        {
            direction = SteerTowards(swarmCenter) * CenterWeight + SteerTowards(swarmHeanding) * HeadingWeight + SteerTowards(separation) * SeparationWeight;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, (Character.transform.position - transform.position).normalized, out hit))
        {
            if(hit.collider.tag == "Character")
            {
                direction += SteerTowards(Character.transform.position - transform.position) * TargetWeight;
            }
        }

        Ray ray = new Ray(this.transform.position, this.transform.forward);
        if (Physics.SphereCast(ray, 1.25f, out hit, VisionRadius, 1))
        {
            direction += SteerTowards(hit.normal);
        }

        direction.x = 0;
        Velocity += direction * Time.deltaTime;
        float speed = Velocity.magnitude;
        Vector3 dir = Velocity / speed;
        speed = Mathf.Clamp(speed, 1, Speed);
        Velocity = dir * speed;

        transform.position += Velocity * Time.deltaTime;
        transform.forward = dir;

        
    }


    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * Speed - Velocity;
        return v;
    }
}
