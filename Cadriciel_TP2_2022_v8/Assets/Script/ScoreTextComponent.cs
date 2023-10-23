using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextComponent : MonoBehaviour
{
    [SerializeField] Text text;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(ScoreManager.Instance.Score);

        ScoreManager.Instance.OnScoreUpdated.AddListener(UpdateScore);
    }

    void UpdateScore(int newScore)
    {
        text.text = newScore.ToString();
    }
}
