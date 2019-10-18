using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public float defaultVolume = 0.6f;
    public float volumeRange = 0.08f;
    public float defaultPitch = 1f;
    public float pitchRange = 0.2f;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        audioSource.volume = defaultVolume + Random.Range(-volumeRange, volumeRange);
        audioSource.pitch = defaultPitch + Random.Range(-pitchRange, pitchRange);
    }
}
