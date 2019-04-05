using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    [HideInInspector] public static PostProcessingManager instance;

    [Header("Static Effect Variables")]
    [SerializeField] private float staticEffectGrain; //The value that the grain will be set to when the player is hit

    [SerializeField] private PostProcessProfile redProfile;

    PostProcessProfile defaultProfile;

    [Header("Fade To Black Effect Variables")]

    [SerializeField] private float vignetteIntesityEndValue;

    [SerializeField] private float vignetteSmoothnessEndValue;

    [SerializeField] private float vignetteIntesityTotalTime;

    [SerializeField] private float vignetteSmoothnessTotalTime;


    bool fadingToBlack = true;

    //Volumes
    PostProcessVolume volume;
    Vignette vingetteVolume;
    Grain grainVolume;
    ColorGrading colorGradingVolume;
    AmbientOcclusion ambientOcclusionVolume;

    //Settings variables
    float grainBaseIntesity;
    float vingetteBaseIntestiy;
    float vingetteBaseSmoothness;
    float ambientOcclusionBaseIntesity;
    

    private void Awake() 
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Gets the volume from the main camera
        volume = Camera.main.GetComponent<PostProcessVolume>(); //Gets the post processing volume from the camera

        //Get post prossesing effect volumes
        volume.profile.TryGetSettings(out grainVolume);
        volume.profile.TryGetSettings(out vingetteVolume);
        volume.profile.TryGetSettings(out colorGradingVolume);
        volume.profile.TryGetSettings(out ambientOcclusionVolume);

        //Gets the intensity values
        grainBaseIntesity = grainVolume.intensity.value;
        vingetteBaseIntestiy = vingetteVolume.intensity.value;
        ambientOcclusionBaseIntesity = ambientOcclusionVolume.intensity.value;

        //Gets other values
        vingetteBaseSmoothness = vingetteVolume.smoothness.value;

        //gets the default profile
        defaultProfile = volume.profile;
    }

    public void EnableStaticEffect() //Will enable the static effect
    {
        //grainVolume.intensity.value = staticEffectGrain;
        volume.profile = redProfile;
    }

    public void DisableStaticEffect() //Will disable the static effect
    {
        //grainVolume.intensity.value = grainIntesityDefaultValue;
        volume.profile = defaultProfile;
    }

    public void StartFadeToBlack() //Starts the fade to black effect
    {
        StartCoroutine(FadeToBlackIntensity());
        StartCoroutine(FadeToBlackSmoothness());
    }

    //Normalized from 0 to 1
    public void AdjustSetting(float grainNew, float vingnetteNew, float ambientOcclusionNew, float brightnessNew)
    {
        //Sets the base to the values set by the player
        grainBaseIntesity = grainNew;
        vingetteBaseIntestiy = vingnetteNew;
        ambientOcclusionBaseIntesity = ambientOcclusionNew;

        //Applies to the volume
        grainVolume.intensity.value = grainBaseIntesity;
        vingetteVolume.intensity.value = vingetteBaseIntestiy;
        ambientOcclusionVolume.intensity.value = ambientOcclusionBaseIntesity;

        colorGradingVolume.colorFilter.value *= brightnessNew;
    }

    private IEnumerator FadeToBlackIntensity() //Fades the intesity
    {
        float time = 0f;

        while(time < vignetteIntesityTotalTime)
        {
            vingetteVolume.intensity.value  = Mathf.Lerp(vingetteBaseIntestiy, vignetteIntesityEndValue, (time / vignetteIntesityTotalTime));
            time+= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeToBlackSmoothness() //Fades the smoothness
    {
        float time = 0f;

        while(time < vignetteSmoothnessTotalTime)
        {
            vingetteVolume.smoothness.value  = Mathf.Lerp(vingetteBaseSmoothness, vignetteSmoothnessEndValue, (time / vignetteSmoothnessTotalTime));
            time+= Time.deltaTime;
            yield return null;
        }
    }
}
