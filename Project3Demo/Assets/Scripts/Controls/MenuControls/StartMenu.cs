using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MenuItemCall {
    
    [SerializeField] string nextScene;

    void Start(){
        nextMenu = false;
    }

    public override void ClickCall(){
        //SceneManager.LoadScene(nextScene);
        LevelSelectManager.instance.NewGame();
    }

    public override void OnHighlight(float x){}
}
