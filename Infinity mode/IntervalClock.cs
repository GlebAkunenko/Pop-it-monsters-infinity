using System.Collections;
using UnityEngine;
using System;

public class IntervalClock : MonoBehaviour
{
    private int interval;
    private int timeOffset;

    private IEnumerator coroutine;

    private IEnumerator GetEnumerator()
    {
        if (timeOffset != 0 && interval - timeOffset > 0) {
            yield return new WaitForSeconds(interval - timeOffset);
            ClockCall?.Invoke();
        }
        while (true) {
            yield return new WaitForSeconds(interval);
            ClockCall?.Invoke();
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="timeInterval">time between ClockCall`s in seconds </param>
    /// <param name="timeNowOffset">time offset before the first ClockCall</param>
    public void Run(int timeInterval, int timeNowOffset = 0)
    {
        if (IsWork)
            throw new Exception("Corutine already has been started.");

        interval = timeInterval;
        timeOffset = timeNowOffset;

        IsWork = true;
        coroutine = GetEnumerator();
        StartCoroutine(coroutine);
    }

    public void Stop()
    {
        StartCoroutine(coroutine);
        IsWork = false;
    }


    public bool IsWork { get; private set; } = false;

    public event Action ClockCall;
}
