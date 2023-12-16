using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip loopingAudioClip;

    private AudioSource mAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!mAudioSource.isPlaying)
        {
            mAudioSource.clip = loopingAudioClip;
            mAudioSource.loop = true;
            mAudioSource.Play();
        }
    }
}
