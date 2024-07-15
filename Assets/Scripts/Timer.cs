using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public float second;
    public float maxSecond;
    public bool isTimerStarted = false;
    public Action OnTimerEnd = null;
    public void StartTimer(float second)
    {
        maxSecond = second;
        isTimerStarted = true;
    }
    public void ResetTimer()
    {
        maxSecond = 0;
        isTimerStarted = false;
    }

    private void Update()
    {
        if (!isTimerStarted) return;
        if (second < maxSecond)
        {
            second += Time.deltaTime;
        }
        else if (second >= maxSecond)
        {
            OnTimerEnd?.Invoke();
        }
    }
}
