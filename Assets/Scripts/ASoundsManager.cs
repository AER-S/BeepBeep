using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class ASoundsManager : Singleton<ASoundsManager>
{
    [SerializeField] private AudioClip FlippingSound;

    [SerializeField] private AudioClip MatchingSuccessSound;

    [SerializeField] private AudioClip MatchingFailedSound;

    [SerializeField] private AudioClip GameOverSound;

    [SerializeField] private AudioSource CardAudioSource;

    [SerializeField] private AudioSource MatchingAudioSource;

    [RunAfterClass(typeof(AGameManager))]
    private void OnEnable()
    {
        AGameManager.Instance.MatchingSuccess += PlaySuccessSound;
        AGameManager.Instance.MatchingFailed += PlayFailSound;
    }

    private void OnDisable()
    {
        AGameManager.Instance.MatchingSuccess -= PlaySuccessSound;
        AGameManager.Instance.MatchingFailed -= PlayFailSound;
    }

    private void PlayFailSound()
    {
        MatchingAudioSource.PlayOneShot(MatchingFailedSound);
    }

    private void PlaySuccessSound()
    {
        MatchingAudioSource.PlayOneShot(MatchingSuccessSound);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
