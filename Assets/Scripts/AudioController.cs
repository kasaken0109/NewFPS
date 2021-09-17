using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;
    [SerializeField] Animator player;
    protected AudioSource sourcew;
    protected AudioSource sourcer;
    [SerializeField] bool randomizePitch = true;
    [SerializeField] float pitchRange = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        sourcew = GetComponents<AudioSource>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        //if (player.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        //{
        //    sourcew.PlayOneShot;
        //}
        //else if (player.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        //{
        //    AudioSource.PlayClipAtPoint(runSound, m_player.transform.position);
        //}
    }

    public void PlayFootstepSE()
    {
        if (randomizePitch)
        {
            sourcew.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);
            ///sourcer.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);
        }
        if (player.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            sourcew.PlayOneShot(sounds[0]);
        }
        else if (player.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            sourcew.PlayOneShot(sounds[1]);
        }
    }

    public void PlayJumpSE()
    {
        sourcew.PlayOneShot(sounds[2]);
    }

    public void PlaySlashSE()
    {
        sourcew.PlayOneShot(sounds[3]);
    }

    public void PlaySpecialSlashSE()
    {
        sourcew.PlayOneShot(sounds[4]);
    }
}
