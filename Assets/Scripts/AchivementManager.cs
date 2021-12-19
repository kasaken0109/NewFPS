using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementManager:MonoBehaviour
{
    public static AchivementManager Instance { get; private set; }

    private static int m_dodgeCount = 0;

    public int DodgeCount { get => m_dodgeCount; set { m_dodgeCount = value; }}
    private void Awake()
    {
        Instance = this;
    }

}
