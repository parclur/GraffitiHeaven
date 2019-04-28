using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitle : MenuItemCall {

    void Start(){
        nextMenu = false;
    }

    public override void ClickCall(){
        SceneManager.LoadScene("TitleWithoutRewired");
    }

    public override void OnHighlight(float x) { }
}
