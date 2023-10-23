using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManagerComponent : MonoBehaviour
{
    public SwarmComponent[] SwarmEnnemies
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        SwarmEnnemies = FindObjectsOfType<SwarmComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        SwarmEnnemies = FindObjectsOfType<SwarmComponent>();
    }
}
