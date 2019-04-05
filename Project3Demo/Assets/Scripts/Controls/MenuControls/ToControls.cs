using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired.UI.ControlMapper;

public class ToControls : MenuItemCall {

    [SerializeField] GameObject controlMapper;
    
    public override void ClickCall(){
        pauseUI = true;
        controlMapper.GetComponent<ControlMapper>().Open();
    }

    public override void OnHighlight(float x){}

    public void StopPausing(){
        pauseUI = false;
    }

}
