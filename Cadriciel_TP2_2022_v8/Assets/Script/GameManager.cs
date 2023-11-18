using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager UIM;
    public GameObject RedKey;
    public GameObject RedDoor;
    public GameObject BlueKey;
    public GameObject BlueDoor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObtainKey(string Tag){
        UIM.ObtainKey(Tag);
        if(Tag=="Red Key"){
            RedKey.SetActive(false);
            //RedDoor.SetActive(false);
        }
        if(Tag=="Blue Key"){
            BlueKey.SetActive(false);
            //BlueDoor.SetActive(false);
        }
    }

    public void openDoor(GameObject gameObject)
    {
        if (gameObject.tag=="Red Door" && !RedKey.activeInHierarchy) {
            gameObject.SetActive(false);
        }
        
        if (gameObject.tag == "Blue Door" && !BlueKey.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    public void BoostManager(string boostType, bool iconEnabled, bool iconGood)
    {
        UIM.BoostManager(boostType,iconEnabled,iconGood);
    }
}
