using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform m_pivot = null;
    [SerializeField] Transform m_UpPivot = null;
    [SerializeField] Transform m_DownPivot = null;
    [SerializeField]Vector3 m_temp;
    GameObject m_player;
    //カメラ上下移動の最大、最小角度です。Inspectorウィンドウから設定してください
    [Range(-0.999f, -0.1f)]
    public float maxYAngle = -0.5f;
    [Range(0.1f, 0.999f)]
    public float minYAngle = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameManager.Player.gameObject; 
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxisRaw("Mouse X");
        float vInput = Input.GetAxisRaw("Mouse Y");
        m_player.transform.Rotate(0, hInput, 0);
        if (m_player.transform.position.y + 5 > Camera.main.transform.position.y && m_player.transform.position.y - 3 < Camera.main.transform.position.y)
        {
            //temp = Camera.main.transform;
            Debug.Log("CameraCanMove");
            Camera.main.transform.LookAt(m_pivot, Vector3.up);
            Camera.main.transform.position += new Vector3(0, -vInput * Time.deltaTime * 3, 0);
            //Debug.Log(Camera.main.transform.position);
        }
        else
        {
            if (m_player.transform.position.y + 3 <= Camera.main.transform.position.y)
            {
                Debug.Log("Up");
                Camera.main.transform.LookAt(m_pivot, Vector3.up);
                Camera.main.transform.position = m_UpPivot.position;
            }
            else
            {
                Debug.Log("Down");
                Camera.main.transform.LookAt(m_pivot, Vector3.up);
                Camera.main.transform.position = m_DownPivot.position;
            }
        }
    }
}
