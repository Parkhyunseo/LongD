using UnityEngine;
using System.Collections;

public class ReusableAudioSource : MonoBehaviour
{
    public AudioSource audioSource;
    public Reusable reusable;

    public void Play(AudioClip audioClip)
    {
        StartCoroutine(CPlay(audioClip));
    }

    public IEnumerator CPlay(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        yield return new WaitForSeconds(audioClip.length);
        SimplePool.Despawn(reusable);
    }
}