using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class NavigateMenu : MonoBehaviour{

    Player diver;

    Color aWhite = new Color(1, 1, 1, .152f);
    Color aRed = new Color(1, 0, 0, .152f);
    Color aBlue = new Color(0, 0, 1, .152f);

    public UnityMenuItem[] menuItems;

    public int selectedItem = 0;
    public int selectedMenu = 0;

    bool editingValue;

    [SerializeField] bool additive;

    float time;

    void Start(){
        diver = ReInput.players.GetPlayer("Diver");
        if(!additive) InvokeRepeating("UpdateNumerical", 0f, .15f);
    }

    void Update(){
        if(time == 0){
            time = 1;
            UpdateNumerical();
        }
        else if(time == 1){
            time = 2;
        }
        else time = 0;
        
        MenuItemCall curItem = menuItems[selectedMenu].items[selectedItem];
        Image curImage = menuItems[selectedMenu].panel[selectedItem];

        if(curItem.pauseUI) return;

        curImage.color = aRed;

        if(editingValue){
            curItem.ClickCall();
            curImage.color = aBlue;
        }

        float x = diver.GetAxis("LeftRightMovement");

        curItem.OnHighlight(x);
        
        //If interacting -use the click call function then check if you need to shift menus.
        if(diver.GetButtonDown("Interact")){
            if(curItem.isSetting){
                editingValue = true;
            }
            curItem.ClickCall();
            if(curItem.nextMenu){
                curImage.color = aWhite;
                for(int i = 0; i < menuItems[selectedMenu].items.Length; i++) {
                    menuItems[selectedMenu].items[i].gameObject.SetActive(false);
                }
                selectedMenu+=curItem.menuOver;
                selectedItem = 0;
                for(int i = 0; i < menuItems[selectedMenu].items.Length; i++) {
                    menuItems[selectedMenu].items[i].gameObject.SetActive(true);
                }
            }
        }

        if(diver.GetButtonDown("MenuBack") && selectedMenu > 0){
            if(!editingValue){
                curImage.color = aWhite;
                for(int i = 0; i < menuItems[selectedMenu].items.Length; i++) {
                    menuItems[selectedMenu].items[i].gameObject.SetActive(false);
                }
                selectedMenu=0;
                selectedItem = 0;
                for(int i = 0; i < menuItems[selectedMenu].items.Length; i++) {
                    menuItems[selectedMenu].items[i].gameObject.SetActive(true);
                }
            }
            else {
                editingValue = false;
            }
        }
    }

    void UpdateNumerical(){
        MenuItemCall curItem = menuItems[selectedMenu].items[selectedItem];
        Image curImage = menuItems[selectedMenu].panel[selectedItem];
        
        if(curItem.pauseUI) return;
        if(editingValue) return;

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
