using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBase : MonoBehaviour,IStartTime,IStopTime
{
    public bool IsGameStopped = false;
    public void StartTime()
    {
        IsGameStopped = false;
    }

    public void StopTime()
    {
        IsGameStopped = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsGameStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameStopped) return;
    }
}
