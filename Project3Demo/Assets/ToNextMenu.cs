using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToNextMenu : MenuItemCall {

    void Start(){
        nextMenu = true;
    }

    public override void ClickCall(){}
    public override void OnHighlight(float x){}
}
