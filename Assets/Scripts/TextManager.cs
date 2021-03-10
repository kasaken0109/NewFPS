using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] public Text gameText;
    GameObject[] target;
    GameObject[] enemy;
    public int targetNum;
    public int enemyNum;
    public int enemyFullNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectsWithTag("Target");
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        targetNum = target.Length;
        enemyNum = enemy.Length;
        enemyFullNum = targetNum + enemyNum;
        gameText.text = "Target Remained :" + (targetNum + enemyNum);
    }
}
