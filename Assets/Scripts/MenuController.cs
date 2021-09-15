using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    GameObject fireScript;
    bool menuFlag = false;
    TimerManager timerManager;
    // Start is called before the first frame update
    void Start()
    {
        fireScript = GameObject.FindGameObjectWithTag("Player");
        fireScript.GetComponent<fire>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameStatus == GameManager.GameState.PLAYERWIN || GameManager.Instance.GameStatus == GameManager.GameState.PLAYERLOSE) return;
        if (Input.GetButtonDown("Menu"))
        {
            if (!menuFlag)
            {
                Time.timeScale = 0;
                menu.SetActive(true);
                GameManager.Instance.GameStatus = GameManager.GameState.STOP;
                menuFlag = true;

            }
            else
            {
                Time.timeScale = 1;
                menu.SetActive(false);
                GameManager.Instance.GameStatus = GameManager.GameState.PLAYING;
                menuFlag = false;
            }
            
        }

        if (!menuFlag)
        {
            Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

    }
    
    public void StartClock()
    {
        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
