using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreComponent : MonoBehaviour
{
    [SerializeField]
    int score;

    public void AddToScore() => ScoreManager.Instance.Score += score;
}
