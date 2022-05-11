using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("ロックオンカメラ")]
    CinemachineVirtualCamera m_lockOnCamera = default;

    [SerializeField]
    [Tooltip("プレイヤーのカメラ")]
    CinemachineVirtualCamera m_playerCamera = default;

    private GameObject[] m_lockOnTargets;

    private GameObject m_player;

    private static int cameraPriority = 15;

    private int lockOnId = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_lockOnTargets = SetSearchTarget("Enemy");
        lockOnId = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            lockOnId = lockOnId == m_lockOnTargets.Length - 1 ? 0 : lockOnId + 1;
            m_lockOnCamera.LookAt = m_lockOnTargets[lockOnId].transform;
            m_lockOnCamera.Priority = cameraPriority;
            Debug.Log(lockOnId == m_lockOnTargets.Length - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            m_playerCamera.transform.LookAt(m_lockOnTargets[lockOnId].transform);
            //m_playerCamera.GetCinemachineComponent<CinemachinePOV>().GetRecenterTarget();
            m_lockOnCamera.Priority = 0;
            lockOnId = 0;
            m_lockOnTargets = SetSearchTarget("Enemy");
        }
    }

    ///// <summary>
    ///// 特定のコンポーネントを持つオブジェクを検索する、重いのであまり多用しない
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <returns></returns>
    //public GameObject[] SetSearchTarget<T>() where T : Component
    //{
    //    return FindObjectsOfType<GameObject>();
    //}

    public GameObject[] SetSearchTarget(string tag)
    {
        var target = GameObject.FindGameObjectsWithTag(tag);
        return target.Where(ta => ta.GetComponent<Rigidbody>()) .OrderBy(t => Vector3.Distance(m_player.transform.position,t.transform.position)).ToArray();
    }
}
