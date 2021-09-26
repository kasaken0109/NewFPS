using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour,IDamage
{
    TargetManager target;
    public void AddDamage(int damage)
    {
        target.HitObject();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GetComponentInChildren<TargetManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
