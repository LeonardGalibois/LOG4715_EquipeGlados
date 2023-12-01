using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TimerController : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI text;
    public StartMenuController SMC;
    int gameTime;
    float totalPauseTime;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        gameTime =(int) (Time.unscaledTime - SMC.startTime - totalPauseTime);
        if ( (gameTime %60) < 10){
            text.text = (gameTime / 60).ToString() + ":0" + (gameTime % 60).ToString();
        }
        else
        {
            text.text = (gameTime / 60).ToString() + ":" + (gameTime % 60).ToString();
        }  
    }

    public void AddPauseTimer(float addedTime)
    {
        totalPauseTime += addedTime;
    }
}
