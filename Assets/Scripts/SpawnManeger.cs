using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManeger : MonoBehaviour
{
    [SerializeField] Transform [] e_spawnPoint = null;
    [SerializeField] GameObject enemy = null;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < e_spawnPoint.Length; i++)
        {
            enemy = Instantiate<GameObject>(enemy, e_spawnPoint[i].position, e_spawnPoint[i].rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
