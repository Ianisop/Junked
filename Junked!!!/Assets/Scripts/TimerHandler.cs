using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    public float duration;
    public float currentTime;
    public string name;
    public bool isDone = true;
    [HideInInspector] public bool active;

    public void StartTimer()
    {
        active = true;
        isDone = false;
        currentTime = 0;
    }

    public void StopTimer()
    {
        // Debug.Log("Timer ended: " + name);
        active = false;
        isDone = true;
    }

    public void UpdateTimer()
    {
        if (active)
        {
            currentTime += Time.deltaTime;
            // Debug.Log("Updating " + name);
            if (currentTime >= duration)
            {
                StopTimer();
            }
        }
    }

    public void RestartTimer(bool start = true)
    {
        isDone = false;
        active = true;
        currentTime = 0;

        if (start)
        {
            StartTimer();
        }
    }

}

public class TimerHandler : MonoBehaviour
{
    public List<Timer> timers;

    void Update()
    {
        foreach (var timer in timers)
        {
            timer.UpdateTimer();
        }
    }

    public void StartTimers()
    {
        foreach (var timer in timers)
        {
            timer.StartTimer();
        }
    }

    public void PauseTimers()
    {
        foreach (var timer in timers)
        {
            timer.active = false;
        }
    }

    public void ResumeTimers()
    {

        foreach (var timer in timers)
        {
            timer.active = true;
        }

    }

    public void AddTimer(Timer timer, bool flag)
    {
        timers.Add(timer);
        if (flag) timer.StartTimer();
    }
}