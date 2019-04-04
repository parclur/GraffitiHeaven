using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class NavigateMenu : MonoBehaviour{

    Player diver;

    Color aWhite = new Color(1, 1, 1, .152f);
    Color aRed = new Color(1, 0, 0, .152f);

    public UnityMenuItem[] menuItems;

    int selectedItem = 0;
    int selectedMenu = 0;

    void Start(){
        diver = ReInput.players.GetPlayer("Diver");
        InvokeRepeating("UpdateNumerical", 0f, .15f);
    }

    void Update(){
        MenuItemCall curItem = menuItems[selectedMenu].items[selectedItem];
        Image curImage = menuItems[selectedMenu].panel[selectedItem];

        if(curItem.pauseUI) return;

        float x = diver.GetAxis("LeftRightMovement");

        curItem.OnHighlight(x);
        
        curImage.color = aRed;

        //If interacting -use the click call function then check if you need to shift menus.
        if(diver.GetButton("Interact")) {
            curItem.ClickCall();
            if(curItem.nextMenu){
                curImage.color = aWhite;
                for(int i = 0; i < menuItems[selectedMenu].items.Length; i++) {
                    menuItems[selectedMenu].items[i].gameObject.SetActive(false);
                }
                selectedMenu++;
                selectedItem = 0;
                for(int i = 0; i < menuItems[selectedMenu].items.Length; i++) {
                    menuItems[selectedMenu].items[i].gameObject.SetActive(true);
                }
            }
        }

        if(diver.GetButton("MenuBack") && selectedMenu > 0){
            curImage.color = aWhite;
            for(int i = 0; i < menuItems[selectedMenu].items.Length; i++) {
                menuItems[selectedMenu].items[i].gameObject.SetActive(false);
            }
            selectedMenu--;
            selectedItem = 0;
            for(int i = 0; i < menuItems[selectedMenu].items.Length; i++) {
                menuItems[selectedMenu].items[i].gameObject.SetActive(true);
            }
        }
    }

    void UpdateNumerical(){
        MenuItemCall curItem = menuItems[selectedMenu].items[selectedItem];
        Image curImage = menuItems[selectedMenu].panel[selectedItem];
        
        if(curItem.pauseUI) return;

        float y = diver.GetAxis("UpDownMovment");

        //Adjust the verticality of the button
        if(y > 0 && selectedItem > 0){
            curImage.color = aWhite;
            selectedItem--;
        }

        if(y < 0 && selectedItem < menuItems[selectedMenu].items.Length - 1){
            curImage.color = aWhite;
            selectedItem++;
        }
    }
}
