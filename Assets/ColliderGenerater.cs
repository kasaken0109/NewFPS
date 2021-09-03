using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderGenerater : MonoBehaviour
{
    public static ColliderGenerater Instance { get; private set; }
    GameObject hitCollider;
    float waitTime;

    private void Awake()
    {
        Instance = this;
    }
    public IEnumerator GenerateCollider(GameObject hitCollider ,float waitTime)
    {
        hitCollider.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        hitCollider.SetActive(false);
    }

    public void StartActiveCollider(GameObject hitCollider, float waitTime)
    {
        StartCoroutine((GenerateCollider(hitCollider, waitTime)));
    }
}
