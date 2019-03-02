using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    [HideInInspector] public static PostProcessingManager instance;

    [Header("Static Effect Variables")]
    [SerializeField] private float staticEffectGrain; //The value that the grain will be set to when the player is hit

    Grain grainVolume;

    float grainIntesityDefaultValue; //The amount of grain that is set by default

    [Header("Fade To Black Effect Variables")]

    [SerializeField] private float vignetteIntesityEndValue;

    [SerializeField] private float vignetteSmoothnessEndValue;

    [SerializeField] private float vignetteIntesityTotalTime;

    [SerializeField] private float vignetteSmoothnessTotalTime;

    Vignette vingetteVolume;

    PostProcessVolume volume;

    float vingetteIntestityDefaultValue;

    float vingetteSmoothnessDefaultValue;

    bool fadingToBlack = true;

    private void Awake() 
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        volume = Camera.main.GetComponent<PostProcessVolume>(); //Gets the post processing volume from the camera
        volume.profile.TryGetSettings(out grainVolume); //Gets the settings for grain volume
        volume.profile.TryGetSettings(out vingetteVolume);

        grainIntesityDefaultValue = grainVolume.intensity.value;
        vingetteIntestityDefaultValue = vingetteVolume.intensity.value;
        vingetteSmoothnessDefaultValue = vingetteVolume.smoothness.value;

        //StartFadeToBlack();
    }

    public void EnableStaticEffect() //Will enable the static effect
    {
        grainVolume.intensity.value = staticEffectGrain;
    }

    public void DisableStaticEffect() //Will disable the static effect
    {
        grainVolume.intensity.value = grainIntesityDefaultValue;
    }

    public void StartFadeToBlack() //Starts the fade to black effect
    {
        StartCoroutine(FadeToBlackIntensity());
        StartCoroutine(FadeToBlackSmoothness());
    }



    private IEnumerator FadeToBlackIntensity() //Fades the intesity
    {
        float time = 0f;

        while(time < vignetteIntesityTotalTime)
        {
            vingetteVolume.intensity.value  = Mathf.Lerp(vingetteIntestityDefaultValue, vignetteIntesityEndValue, (time / vignetteIntesityTotalTime));
            time+= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeToBlackSmoothness() //Fades the smoothness
    {
        float time = 0f;

        while(time < vignetteSmoothnessTotalTime)
        {
            vingetteVolume.smoothness.value  = Mathf.Lerp(vingetteSmoothnessDefaultValue, vignetteSmoothnessEndValue, (time / vignetteSmoothnessTotalTime));
            time+= Time.deltaTime;
            yield return null;
        }
    }
}
