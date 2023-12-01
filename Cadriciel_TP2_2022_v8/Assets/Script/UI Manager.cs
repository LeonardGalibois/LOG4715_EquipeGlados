using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text UIText;
    public RawImage RedPanel;
    public RawImage BluePanel;
    public GameObject speedBoost;
    public GameObject jumpBoost;
    public GameObject PauseScreen;
    public TimerController timerController;

    float pauseTimer=0f;
    float pauseCooldown = 0f;
 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.unscaledTime - pauseCooldown > 0.2f && Input.GetKey("escape"))
        {
            if (pauseTimer == 0f)
            {
                pauseCooldown= Time.unscaledTime;
                pauseTimer = Time.unscaledTime;
                PauseScreen.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else
            {
                Resume();
                pauseCooldown = Time.unscaledTime;
            }
        }

    }
    public void Resume()
    {
        timerController.AddPauseTimer(Time.unscaledTime-pauseTimer);
        pauseTimer = 0f;
        PauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void ObtainKey(string Tag)
    {
        Debug.Log(Tag);
        if (Tag == "Red Key")
        {
            Debug.Log(Tag+" true");
            RedPanel.gameObject.SetActive(true);
            //RedPanel.gameObject.SetActive(!RedPanel.gameObject.activeSelf);

        }
        if (Tag == "Blue Key")
        {
            BluePanel.gameObject.SetActive(true);
        }
    }

    public void BoostManager(string boostType,bool iconEnabled, bool iconGood)
    {
        if (boostType == "speed")
        {
            if (iconEnabled)
            {
                speedBoost.SetActive(true);
                if(iconGood) {
                    speedBoost.GetComponentInChildren<Image>().color = Color.green;
                }
                else
                {
                    speedBoost.GetComponentInChildren<Image>().color = Color.red;
                }
                
            }
            else
            {
                speedBoost.SetActive(false);
            }
        }
        else if (boostType == "jump")
        {
            if (iconEnabled)
            {
                jumpBoost.SetActive(true);
                if (iconGood)
                {
                    jumpBoost.GetComponentInChildren<Image>().color = Color.green;
                }
                else
                {
                    jumpBoost.GetComponentInChildren<Image>().color = Color.red;
                }

            }
            else
            {
                jumpBoost.SetActive(false);
            }
        }
    }
    
}
