using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessSlider : MenuItemCall {

    public Text text;

    void Start(){
        nextMenu = false;
        text = GetComponent<Text>();
        text.text = "Brightness: " + ((int)OptionsManager.instance.brightnessLevel).ToString() + "%";
    }

    public override void ClickCall(){}

    public override void OnHighlight(float x){
        OptionsManager.instance.postProcessingLevel += x;
        text.text = "Brightness: " + ((int)OptionsManager.instance.brightnessLevel).ToString() + "%";
    }
}
