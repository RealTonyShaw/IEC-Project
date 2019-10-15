using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // float original = audioSource.volume;
        audioSource.volume = (float)Random.Range(20, 100) / 100;
        Debug.Log("Volume " + audioSource.volume);
    }
}
