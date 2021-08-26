using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのカメラ移動を処理する
/// </summary>
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    /// <summary>カメラの回転する中心点</summary>
    [SerializeField] Transform m_pivot = null;
    /// <summary>上の修正点 </summary>
    [SerializeField] Transform m_UpPoint = null;
    /// <summary>下の修正点 </summary>
    [SerializeField] Transform m_DownPoint = null;
    GameObject m_player;
    bool m_isMoveActive = true;
    public void SetMoveActive(bool IsMoveActive) { m_isMoveActive = IsMoveActive; } 

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameManager.Player.gameObject; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isMoveActive) return;
        float hInput = Input.GetAxisRaw("Mouse X");
        float vInput = Input.GetAxisRaw("Mouse Y");
        m_player.transform.Rotate(0, hInput, 0);
        if (m_player.transform.position.y + 5 > Camera.main.transform.position.y && m_player.transform.position.y - 3 < Camera.main.transform.position.y)
        {
            Camera.main.transform.LookAt(m_pivot, Vector3.up);
            Camera.main.transform.position += new Vector3(0, -vInput * Time.deltaTime * 3, 0);
        }
        else
        {
            if (m_player.transform.position.y + 3 <= Camera.main.transform.position.y)
            {
                Camera.main.transform.LookAt(m_pivot, Vector3.up);
                Camera.main.transform.position = m_UpPoint.position;
            }
            else
            {
                Camera.main.transform.LookAt(m_pivot, Vector3.up);
                Camera.main.transform.position = m_DownPoint.position;
            }
        }
    }
}
