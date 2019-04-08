using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUIBattery : MonoBehaviour {

    [SerializeField] Texture[] flares;

    RawImage image;
    FlareGun flareGun;

    void Start(){
        image = GetComponent<RawImage>();
        flareGun = GameObject.FindGameObjectWithTag("FlareGun").GetComponent<FlareGun>();
    }
    
    void Update(){
        image.texture = flares[flareGun.flareCount];
    }
}
