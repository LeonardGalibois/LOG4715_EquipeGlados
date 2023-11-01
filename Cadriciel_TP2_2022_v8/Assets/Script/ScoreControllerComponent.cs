using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreControllerComponent : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        ScoreManager.Instance.OnScoreUpdated.AddListener(OnScoreUpdated);

        OnScoreUpdated(ScoreManager.Instance.Score);
    }

    void OnScoreUpdated(int score) => text.text = score.ToString();
}
