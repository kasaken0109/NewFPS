using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject menu = default;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera = default;

    private bool menuFlag = true;
    
    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameStatus == GameManager.GameState.PLAYERWIN || GameManager.Instance.GameStatus == GameManager.GameState.PLAYERLOSE) return;
        if (Input.GetButtonDown("Menu"))
        {
            SetMenu();
        }

        if (!menuFlag)
        {
            Cursor.visible = false;
            virtualCamera.enabled = true;
            //Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            virtualCamera.enabled = false;
        }

    }

    public void SetMenu()
    {
        Time.timeScale = menuFlag ? 1 : 0;
        menu.SetActive(!menuFlag);
        GameManager.Instance.GameStatus = menuFlag ? GameManager.GameState.PLAYING : GameManager.GameState.STOP;
        menuFlag = menuFlag ? false : true;
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
