using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class NavigateMenu : MonoBehaviour{

    Player diver;

    public UnityMenuItem[] menuItems;

    int selectedItem = 0;
    int selectedMenu = 0;

    void Start(){
        diver = ReInput.players.GetPlayer("Diver");
    }

    void Update(){
        float y = diver.GetAxis("UpDownMovment");
        float x = diver.GetAxis("LeftRightMovement");

        if(y > 0 && selectedItem > 0){
            y--;
        }
        if(y < 0 && selectedItem < menuItems[selectedMenu].items.Length){
            y++;
        }
    }
}
