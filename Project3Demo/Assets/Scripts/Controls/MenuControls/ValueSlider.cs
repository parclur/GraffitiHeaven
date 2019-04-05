using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueSlider : MenuItemCall {

    public Text text;
    [SerializeField] string vName;
    [SerializeField] OptionsManager.SettingsName sName;

    void Start(){
        nextMenu = false;
        text = GetComponent<Text>();
        text.text = name + ((int)OptionsManager.instance.settingsLevel[(int)sName]).ToString() + "%";
    }

    public override void ClickCall(){}

    public override void OnHighlight(float x){
        OptionsManager.instance.settingsLevel[(int)sName] += x;
        text.text = name + ((int)OptionsManager.instance.settingsLevel[(int)sName]).ToString() + "%";
    }
}
