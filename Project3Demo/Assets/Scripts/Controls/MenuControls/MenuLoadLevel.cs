using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoadLevel : MenuItemCall
{
    [SerializeField] int levelToLoad;

    void Start()
    {
        nextMenu = false;
    }

    public override void ClickCall()
    {
        switch (levelToLoad)
        {
            case 1:
                LevelSelectManager.instance.LoadLevel1();
                break;
            case 2:
                LevelSelectManager.instance.LoadLevel2();

                break;
            case 3:
                LevelSelectManager.instance.LoadLevel3();

                break;
            case 4:
                LevelSelectManager.instance.LoadLevel4();

                break;
            case 5:
                LevelSelectManager.instance.LoadLevel5();

                break;
            default:
                Debug.Log("Invalid level selection");
                break;

        }

    }

    public override void OnHighlight(float x) { }
}