using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameMenu : MenuItemCall
{
    void Start()
    {
        nextMenu = false;
    }

    public override void ClickCall()
    {
        //SceneManager.LoadScene(nextScene);
        Application.Quit();
    }

    public override void OnHighlight(float x) { }
}
