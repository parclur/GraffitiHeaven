using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public static AudioManager instance;

    [Header("Audio Data")]

    [SerializeField] private string[] clipNames;

    [SerializeField] private AudioClip[] clips;

    private Dictionary<string, AudioClip> _clips;

    [Header("Player Audio Location")]

    [SerializeField] private GameObject playFrom;

    private AudioSource main;

    private void Awake()
    {
        instance = this;

        _clips = new Dictionary<string, AudioClip>();

        int queueCount = clipNames.Length;

        if (clipNames.Length != clips.Length)
        {
            queueCount = Mathf.Min(clips.Length, clipNames.Length);
        }

        for (int count = 0; count < queueCount; count++)
        {
            _clips.Add(clipNames[count], clips[count]);
        }
    }

    private void Start()
    {
        main = playFrom.AddComponent<AudioSource>();
    }

    public AudioSource AddAudio(string name, float volume = 0.5f, float delay = 0f, bool loop = false, GameObject attachedTo = null)
    {
        AudioSource play;

        if (attachedTo != null)
        {
            play = attachedTo.AddComponent<AudioSource>();
        }
        else
        {
           play = playFrom.AddComponent<AudioSource>();
        }
       
        play.playOnAwake = false;

        play.clip = _clips[name];
        play.volume = volume;
        play.loop = loop;

        play.PlayDelayed(delay);

        return play;
    }

    public void PlayOneShot(string name, float volume = 0.5f, float delay = 0f)
    {
        if (delay != 0.0)
            StartCoroutine(PlayOneShotDelayed(name, volume, delay));
        else
            main.PlayOneShot(_clips[name], volume);
    }

    private IEnumerator PlayOneShotDelayed(string name, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        main.PlayOneShot(_clips[name], volume);
    }
}