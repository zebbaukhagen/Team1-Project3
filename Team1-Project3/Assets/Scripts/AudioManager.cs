using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSource2;

    public void PlayClip(AudioClip clipToPlay)
    {
        // Plays the given clip on the backup audioSource
        audioSource2.PlayOneShot(clipToPlay);
    }
    
}
