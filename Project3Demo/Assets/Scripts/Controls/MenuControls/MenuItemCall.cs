using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemCall : MonoBehaviour {
    
    [HideInInspector] public bool nextMenu;
    [HideInInspector] public bool pauseUI;
    [HideInInspector] public bool isSetting;
    [HideInInspector] public int menuOver;

    public virtual void ClickCall(){}

    public virtual void OnHighlight(float x){}

}
