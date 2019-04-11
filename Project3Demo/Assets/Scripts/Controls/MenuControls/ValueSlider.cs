using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueSlider : MenuItemCall {

    public Text text;
    [SerializeField] string vName;
    [SerializeField] OptionsManager.SettingsName sName;
    float nX;

    void Start(){
        nextMenu = false;
        isSetting = true;
        text = GetComponent<Text>();
        text.text = name + ((int)OptionsManager.instance.settingsLevel[(int)sName]).ToString() + "%";
    }

    public override void ClickCall(){
        OptionsManager.instance.settingsLevel[(int)sName] += nX;
        text.text = name + ((int)OptionsManager.instance.settingsLevel[(int)sName]).ToString() + "%";
    }

    public override void OnHighlight(float x){
        nX = x;
    }
}
