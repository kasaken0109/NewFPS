using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("左ひざのHint用Transform")]
    private Transform leftKneeTransform;

    [SerializeField]
    [Tooltip("右ひざのHint用Transform")]
    private Transform rightKneeTransform;

    private Animator m_anim = default;

    private float IKValue;

    // Start is called before the first frame update
    void Start()
    {
        IKValue = 1;
        m_anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnAnimatorIK(int layerIndex)
    {
        m_anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, IKValue);
        m_anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, IKValue);
        m_anim.SetIKHintPosition(AvatarIKHint.RightKnee, rightKneeTransform.position);
        m_anim.SetIKHintPosition(AvatarIKHint.LeftKnee, leftKneeTransform.position);
    }

    public void SetIKValue(float weight)
    {
        if (weight < 0 || weight > 1) IKValue = 0;
        IKValue = weight;
    }
}
