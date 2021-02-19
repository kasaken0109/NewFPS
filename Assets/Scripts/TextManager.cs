using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] public Text gameText;
    GameObject[] target;
    public int targetNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectsWithTag("Target");
        targetNum = target.Length;
        gameText.text = "Target Remained :" + targetNum;
    }
}
