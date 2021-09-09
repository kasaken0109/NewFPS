using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDisplay : MonoBehaviour
{
    [SerializeField] Text m_time = null;
    [SerializeField] Text m_rank = null;
    [SerializeField] GameObject[] m_buttons;
    int clearTime;
    int maxTime;
    string rank;
    int clearRank;
    // Start is called before the first frame update
    void Start()
    {
        clearTime = PlayerPrefs.GetInt("TimeScore");
        maxTime = PlayerPrefs.GetInt("MaxTime");
        clearRank = (clearTime * 10) / maxTime;
        Debug.Log(clearRank);
        switch (clearRank)
        {
            case 0:rank = "SS";
                break;
            case 1:
                rank = "S";
                break;
            case 2:
                rank = "A";
                break;
            case 3:
                rank = "B";
                break;
            case 4:
                rank = "C";
                break;
            case 5:
                rank = "D";
                break;
        }
        StartCoroutine(nameof(DisplayResult));
    }

    IEnumerator DisplayResult()
    {
        float time = 0;
        while (time < 3)
        {
            if (Input.GetButton("Fire1")) break;
            m_time.text = Random.Range(1, 1000).ToString();
            time += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        m_time.text = clearTime.ToString();
        yield return new WaitForSeconds(0.5f);
        m_rank.text = rank;
        foreach (var item in m_buttons)
        {
            yield return new WaitForSeconds(0.5f);
            item.SetActive(true);
        }

    }
}
