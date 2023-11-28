using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    public void StartGame()
    {
        Time.timeScale = 1.0f;
        Debug.Log("gamestarted");
        this.gameObject.SetActive(false);
    }

}
