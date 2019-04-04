using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MenuItemCall {

    public Text text;

    void Start(){
        nextMenu = false;
        text = GetComponent<Text>();
        text.text = "Sound Volume: " + ((int)OptionsManager.instance.soundVolume).ToString();
    }

    public override void ClickCall(){}

    public override void OnHighlight(float x){
        OptionsManager.instance.soundVolume += x;
        text.text = "Sound Volume: " + ((int)OptionsManager.instance.soundVolume).ToString();
    }
}
