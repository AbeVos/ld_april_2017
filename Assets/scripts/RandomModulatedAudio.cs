using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomModulatedAudio : MonoBehaviour
{
    [SerializeField]
    private Dictionary<string,AudioClip[]> namedAudioClips;
    [SerializeField]
    private float pitchModulation;

    private AudioSource audioSource;

    public void Play(string parameterName = "")
    {
        audioSource.pitch = Random.Range(1 - pitchModulation, 1 + pitchModulation);
        
    }

    // Use this for initialization
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        foreach (var clips in namedAudioClips.Values)
        {
            foreach (var audioClip in clips)
            {
                audioClip.LoadAudioData();
            }
        }
    }
}
