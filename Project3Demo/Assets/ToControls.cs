using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToControls : MenuItemCall {

    [SerializeField] GameObject controlMapper;
    
    public override void ClickCall(){
        pauseUI = true;
        controlMapper.SetActive(true);
    }

    public override void OnHighlight(float x){}

    public void StopPausing(){
        pauseUI = false;
        controlMapper.SetActive(false);
    }

}
