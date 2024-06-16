
using UnityEngine;

public class ASoundsManager : Singleton<ASoundsManager>
{
    [SerializeField] private AudioClip FlippingSound;

    [SerializeField] private AudioClip MatchingSuccessSound;

    [SerializeField] private AudioClip MatchingFailedSound;

    [SerializeField] private AudioClip GameOverSound;

    [SerializeField] private AudioSource CardAudioSource;

    [SerializeField] private AudioSource MatchingAudioSource;
    
    private void OnEnable()
    {
        AGameManager.Instance.MatchingSuccess += PlaySuccessSound;
        AGameManager.Instance.MatchingFailed += PlayFailSound;
        AGameManager.Instance.GameOver += PlayGameOverSound;
    }


    private void OnDisable()
    {
        AGameManager.Instance.MatchingSuccess -= PlaySuccessSound;
        AGameManager.Instance.MatchingFailed -= PlayFailSound;
        AGameManager.Instance.GameOver -= PlayGameOverSound;
    }
    private void PlayGameOverSound(bool value)
    {
        MatchingAudioSource.PlayOneShot(GameOverSound);
    }

    private void PlayFailSound()
    {
        MatchingAudioSource.PlayOneShot(MatchingFailedSound);
    }

    private void PlaySuccessSound()
    {
        MatchingAudioSource.PlayOneShot(MatchingSuccessSound);
    }

    public void PlayFlippingSound()
    {
        CardAudioSource.PlayOneShot(FlippingSound);
    }
}
