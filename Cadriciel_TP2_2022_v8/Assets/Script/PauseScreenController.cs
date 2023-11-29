using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    float cooldown;
    private void OnEnable()
    {
        Time.timeScale= 0.0f;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        this.gameObject.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    void Update()
    {

        if (Time.unscaledTime - cooldown > 0.5f && Input.GetKey("escape"))
        {
            cooldown = Time.unscaledTime;
            Resume();
        }
    }
}
