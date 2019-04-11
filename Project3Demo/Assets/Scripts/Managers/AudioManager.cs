using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public static AudioManager instance;

    private readonly string[] data =
    {
        "Motor1, electric_motor",
        "Motor2, electric_motor2",
        "Footsteps, footsteps",
        "Ambience1, 193822__jhumbucker__deep-sea-ambience",
        "Ambience2, underwater_ambience",
        "Ambience3, underwater_ambience2",
        "Soundscape1, soundscape",
        "SlowBreathing1, slow_breathing",
        "SlowBreathing2, 324562__univ-lyon3__saly-margaux-2015-2016-underwater-breathing",
        "WaterMotion, 262754__klankbeeld__water-sand-moving-003-140811-0365",
        "WaveSounds, 267245__klankbeeld__wave-hydrophone-001",
        "MetalHit1, close_metal_hit",
        "MetalHit2, distant_metal_hit",
        "MetalPush, 352745__rosebugg__fence",
        "MetalCrash, metal_crashing",
        "MetalDoorClose, metal_door_close",
        "MetalDoorOpen, metal_door_open",
        "ImpactE, underwater_impact",
        "Impact, unmodified_impact",
        "SinkingCrash, 232021__ubehag__stereotypical-sinking-submarine-sound-no-reverb",
        "HullCreak, 412766__ohnobones__underwater-hullcreaking",
        "Flare, flare",
        "BurningFlare, burning_flare",
        "FlareAmmoBoxPickup, 399122__fastson__magazinerelease-01",
        "EmptyFlareGunClick, 170272__knova__gun-click",
        "Monster1, monster_1",
        "Monster1e, monster_1_e",
        "Monster2, monster_2",
        "Monster2e, monster_2_e",
        "Monster3, monster_3",
        "Monster3e, monster_3_e",
        "Monster4, monster_4",
        "Monster4e, monster_4_e",
        "Monster5, monster_5",
        "Monster5e, monster_5_e",
        "Monster6, monster_6",
        "Monster6e, monster_6_e",
        "Monster7, monster_7",
        "Monster7e, monster_7_e",
        "Monster8, monster_8",
        "Monster8e, monster_8_e",
        "Monster9, monster_9",
        "Monster9e, monster_9_e",
        "Monster10, monster_10",
        "Monster10e, monster_10_e",
        "SeaMonster1, 151852__carroll27__deep-sea-creature-noise-ambient-sound-of-the-deep",
        "SeaMonster2, 395476__chilljeremy__sea-monster",
        "Groan, 260637__rcarpo__deep-sea-groan"
    };

    private Dictionary<string, AudioClip> clips;

    [SerializeField] private GameObject mainAudio;

    [SerializeField] private bool debuggingMode = false;

    private AudioSource main;

    private void Awake()
    {
        instance = this;

        clips = new Dictionary<string, AudioClip>();

        // Read in audio queue data line by line in the format:
        //  ClipName, clip_file_location
        foreach (string path in data)
        {
            int split = path.IndexOf(','); ;
            clips[path.Substring(0, split)] = Resources.Load<AudioClip>("Audio/" + path.Substring(split + 2));              
        }

        if (debuggingMode)
        {
            foreach (KeyValuePair<string, AudioClip> pair in clips)
            {
                Debug.Log(pair.Key + ", " + pair.Value.name);
            }
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