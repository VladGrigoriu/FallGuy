using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float difficultyScale = 1f;

    public float GetDifficultyScale()
    {
        return difficultyScale;
    }

    public void SetDifficultyScale(float difficulty)
    {
        difficultyScale = difficulty;
    }
}
