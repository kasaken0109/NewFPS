using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] AudioClip[] m_click;
    [SerializeField] AudioClip m_frostWall;
    [SerializeField] AudioClip m_playerHit;
    [SerializeField] AudioClip m_roar;
    [SerializeField] AudioClip m_shoot;
    [SerializeField] AudioClip m_heal;
    [SerializeField] AudioClip m_fireB;
    [SerializeField] AudioClip m_move;
    [SerializeField] AudioClip m_dodge;
    [SerializeField] AudioClip m_blizzard;
    [SerializeField] AudioClip m_god;
    [SerializeField] AudioClip m_charge;
    [SerializeField, Range(0, 1f)] float m_seVolume = 0.5f;
    [SerializeField, Range(0, 1f)] float m_bgmVolume = 0.5f;
    [SerializeField] AudioSource m_bgm;
    AudioSource source;

    public float GetSeVolume { get => m_seVolume; }
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
        m_seVolume = PlayerPrefs.GetFloat("SEVolume");
        m_seVolume = PlayerPrefs.GetFloat("BGMVolume");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClick()
    {
        source.PlayOneShot(m_click[0],m_seVolume);
    }

    public void PlayClick(int index)
    {
        if (index > m_click.Length) index = 0;
        source.PlayOneShot(m_click[index], m_seVolume);
    }

    public void PlayRoar()
    {
        source.PlayOneShot(m_roar, m_seVolume * 2);
    }

    public void PlayMove()
    {
        source.PlayOneShot(m_move, m_seVolume * 2);
    }

    public void PlayHeal()
    {
        source.PlayOneShot(m_heal, m_seVolume);
    }

    public void PlayFrostWall()
    {
        source.PlayOneShot(m_frostWall, m_seVolume);
    }
    public void PlayFireB()
    {
        source.PlayOneShot(m_fireB, m_seVolume);
    }

    public void PlayDodge()
    {
        source.PlayOneShot(m_dodge, m_seVolume);
    }

    public void PlayFrost()
    {
        source.PlayOneShot(m_god, m_seVolume * 2);
    }
    public void PlayShoot()
    {
        source.PlayOneShot(m_shoot, m_seVolume);
    }

    public void PlayCharge()
    {
        source.PlayOneShot(m_charge, m_seVolume);
    }

    public void StopSE()
    {
        source.Pause();
    }

    public void PlayHit(AudioClip hit)
    {
        source.PlayOneShot(hit, m_seVolume);
    }
    public void PlayPlayerHit()
    {
        source.PlayOneShot(m_playerHit, m_seVolume);
    }

    public void PlayHit(AudioClip hit, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(hit, pos,2);
    }

    /// <summary>
    /// SEの音量を調整する
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetSEVolume(float volume)
    {
        if(volume > 1f)
        {
            Debug.LogError("SE音量が適切ではありません");
            volume = 1f;
        }
        source.volume = volume;
    }

    /// <summary>
    /// BGMの音量を調整する
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetBGMVolume(float volume)
    {
        if (volume > 1f)
        {
            Debug.LogError("BGM音量が適切ではありません");
            volume = 1f;
        }
        m_bgm.volume = volume;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("SEVolume", m_seVolume);
        PlayerPrefs.SetFloat("BGMVolume", m_bgmVolume);
        PlayerPrefs.Save();
    }
}
