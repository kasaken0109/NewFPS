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
        //sourcer = GetComponents<AudioSource>()[1];
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
            sourcew.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
            Debug.Log("walk");
        }
        else if (player.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            sourcew.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
            Debug.Log("run");
        }
    }
}
