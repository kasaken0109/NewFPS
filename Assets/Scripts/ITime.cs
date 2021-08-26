using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITime : MonoBehaviour
{
    interface IStopTime
    {
        void StopTime();
    }

    // Update is called once per frame
    interface IStartTime
    {
        void StartTime();
    }
}
