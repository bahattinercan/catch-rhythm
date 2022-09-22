using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuMusicButtonUI : MonoBehaviour
{
    int cristalCost;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Button);
    }

    public void SetCristalCost(int value)
    {
        cristalCost = value;
    }

    public void Button()
    {
        string[] names = name.Split('-'); // index 1 is id
        int id = int.Parse(names[1]);
        

        if (names[0] == "cristalButton")
        {
            Transform musicPanel = transform.parent.parent.parent;
            Menu.instance.CristalButton(id,cristalCost,musicPanel);
        }            
        else if (names[0] == "reklamButton")
            Menu.instance.ReklamButton(id);
        else
            Menu.instance.PlayButton(id);
    }
}
