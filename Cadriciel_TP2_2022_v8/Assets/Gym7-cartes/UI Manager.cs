using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UIManager : MonoBehaviour
{
    public Text UIText;
    public RawImage RedPanel;
    public RawImage BluePanel;

    
 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObtainKey(string Tag){
        if (Tag == "Red Key")
        {
            RedPanel.gameObject.SetActive(true);

        }
        if (Tag == "Blue Key")
        {
            BluePanel.gameObject.SetActive(true);
        }
    }
}
