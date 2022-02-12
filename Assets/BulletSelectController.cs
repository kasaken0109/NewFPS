using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSelectController : MonoBehaviour
{
    [SerializeField]
    private Image m_image;
    // Start is called before the first frame update
    void Start()
    {
        m_image.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3")) m_image.gameObject.SetActive(true);
        else if(Input.GetButtonUp("Fire3")) m_image.gameObject.SetActive(false);
    }
}
