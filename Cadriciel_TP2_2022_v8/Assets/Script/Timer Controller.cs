using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TimerController : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI text;
    public StartMenuController SMC;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if ( (((int)Time.unscaledTime - SMC.startTime) %60) < 10){
            text.text = (((int)Time.unscaledTime- SMC.startTime) / 60).ToString() + ":0" + (((int)Time.unscaledTime- SMC.startTime) % 60).ToString();
        }
        else
        {
            text.text = (((int)Time.unscaledTime- SMC.startTime) / 60).ToString() + ":" + (((int)Time.unscaledTime - SMC.startTime) % 60).ToString();
        }  
    }
}
