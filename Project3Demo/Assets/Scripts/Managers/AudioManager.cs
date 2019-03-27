using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public static AudioManager instance;

    [SerializeField] private string audioDataFile = "audio_data.txt";

    [SerializeField] private string path = "Assets/Resources/Audio";

    private Dictionary<string, AudioClip> clips;

    [SerializeField] private GameObject mainAudio;

    private AudioSource main;

    private void Awake()
    {
        instance = this;

        clips = new Dictionary<string, AudioClip>();

        StreamReader read = new StreamReader(path + audioDataFile);

        // Read in audio queue data line by line in the format:
        //  ClipName, clip_file_location.clip_type

        while (!read.EndOfStream)
        {
            string line = read.ReadLine();
            int split = line.IndexOf(',');
            clips[line.Substring(0, split)] = Resources.Load<AudioClip>(path + line.Substring(split));          
        } 
    }

    private void Start()
    {
        main = mainAudio.AddComponent<AudioSource>();
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
           play = mainAudio.AddComponent<AudioSource>();
        }
       
        play.playOnAwake = false;

        play.clip = clips[name];
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
            main.PlayOneShot(clips[name], volume);
    }

    private IEnumerator PlayOneShotDelayed(string name, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        main.PlayOneShot(clips[name], volume);
    }
}