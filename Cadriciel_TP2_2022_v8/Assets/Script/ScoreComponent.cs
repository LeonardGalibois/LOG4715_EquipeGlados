using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreComponent : MonoBehaviour
{
    [SerializeField]
    int score;

    public void AddToScore()
    {
        if (ScoreManager.Instance is not null) ScoreManager.Instance.Score += score;
    }
}
