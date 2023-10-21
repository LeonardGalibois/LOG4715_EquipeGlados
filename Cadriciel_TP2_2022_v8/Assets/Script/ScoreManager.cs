using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager: MonoBehaviour
{
    UnityEvent<int> OnScoreUpdated;

    static public ScoreManager Instance { private set; get; }

    int score;
    public int Score
    {
        set
        {
            score = value;
            OnScoreUpdated.Invoke(score);
        }
        get => score;
    }

    private void Awake()
    {
        if (Instance is null) Instance = this;
        else Destroy(this);
    }
}
