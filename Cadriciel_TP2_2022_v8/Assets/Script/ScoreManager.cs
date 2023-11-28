using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager: MonoBehaviour
{
    public UnityEvent<int> OnScoreUpdated;
    private AudioSource source;

    static public ScoreManager Instance { private set; get; }

    int score;
    public int Score
    {
        set
        {
            score = value;
            OnScoreUpdated?.Invoke(score);
            source.Play();
        }
        get => score;
    }

    private void Awake()
    {
        if (Instance is null) Instance = this;
        else Destroy(this);
        source = GetComponent<AudioSource>();
    }
}
