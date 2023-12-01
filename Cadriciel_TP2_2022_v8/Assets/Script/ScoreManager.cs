using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager: MonoBehaviour
{
    public UnityEvent<int> OnScoreUpdated;

    [SerializeField]
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
        Instance = this;
    }
}
