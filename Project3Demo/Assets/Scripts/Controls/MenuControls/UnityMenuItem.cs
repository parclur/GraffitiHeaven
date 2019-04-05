using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityMenuItem : MonoBehaviour {
    
    public MenuItemCall[] items;
    public Image[] panel;

    void Start(){

        panel = new Image[items.Length];

        for(int i = 0; i < items.Length; i++){
            panel[i] = items[i].GetComponentInChildren<Image>();
        }
    }
}
