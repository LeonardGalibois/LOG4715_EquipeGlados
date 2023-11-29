using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    public GameObject MainMenu;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Time.timeScale = 0.0f;
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }


}
