using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MenuItemCall
{

    

    void Start()
    {
        nextMenu = false;
    }

    public override void ClickCall()
    {
        //SceneManager.LoadScene(nextScene);
        LevelSelectManager.instance.LoadGame();
    }

    public override void OnHighlight(float x) { }
}
