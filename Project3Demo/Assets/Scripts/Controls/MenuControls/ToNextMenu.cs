using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToNextMenu : MenuItemCall {

    [SerializeField] int menuToGoTo = 0;

    void Start(){
        nextMenu = true;
        menuOver = menuToGoTo;
    }

    public override void ClickCall(){  }
    public override void OnHighlight(float x){}
}
