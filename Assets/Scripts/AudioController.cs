using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの音の処理を管理する
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] m_sounds;

    [SerializeField]
    private Animator m_anim = default;

    [SerializeField]
    private float pitchRange = 0.1f;

    private AudioSource sourcew;
    private AudioSource sourcer;
    
    // Start is called before the first frame update
    void Start()
    {
        sourcew = GetComponents<AudioSource>()[0];
    }

    public void PlayFootstepSE()
    {
        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            sourcew.PlayOneShot(m_sounds[0]);
        }
        else if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            sourcew.PlayOneShot(m_sounds[1]);
        }
    }

    public void PlayJumpSE()
    {
        sourcew.PlayOneShot(m_sounds[2]);
    }

    public void PlaySlashSE()
    {
        sourcew.PlayOneShot(m_sounds[3]);
    }

    public void PlaySpecialSlashSE()
    {
        sourcew.PlayOneShot(m_sounds[4]);
    }
}
