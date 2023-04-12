using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Attack[] attacks;

    private int waveIndex = 0;

    public void NextWave(bool firstWave = false)
    {
        if (firstWave)
        {
            waveIndex = 0;
        }
        else
        {
            attacks[waveIndex].Stop();
            waveIndex++;
        }

        attacks[waveIndex].Run();
    }
}
