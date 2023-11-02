using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashThrough : MonoBehaviour
{
    bool CanDash { get; set; }

    [SerializeField]
    Collider m_Collider;

    [SerializeField]
    float dashCooldown = 1f;


    // Start is called before the first frame update
    void Start()
    {
        m_Collider = GetComponent<Collider>();
        CanDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Dash") && CanDash)
        {
            CanDash = false;
            StartCoroutine(PulseCollider());
            Debug.Log("Dash");
        }
    }

    IEnumerator PulseCollider()
    {
        m_Collider.enabled = false;
        yield return new WaitForSeconds(0.3f);
        m_Collider.enabled = true;
        yield return new WaitForSeconds(dashCooldown*1.1f);
        CanDash = true;
    }
}
