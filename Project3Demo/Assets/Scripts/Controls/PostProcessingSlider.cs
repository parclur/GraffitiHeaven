using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostProcessingSlider : MenuItemCall {

    public Text text;

    void Start(){
        nextMenu = false;
        text = GetComponent<Text>();
        text.text = "Post-Processing: " + ((int)OptionsManager.instance.postProcessingLevel).ToString() + "%";
    }

    public override void ClickCall(){}

    public override void OnHighlight(float x){
        OptionsManager.instance.postProcessingLevel += x;
        text.text = "Post-Processing: " + ((int)OptionsManager.instance.postProcessingLevel).ToString() + "%";
    }
}
