using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MusicCristalButtonUI : MonoBehaviour
{
    private int cristalCost;
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        Menu.instance.OnCristalChanged += OnCristalChanged;
    }

    public void SetCristalCost(int value)
    {
        cristalCost = value;
    }

    private void OnCristalChanged(int cristals)
    {
        if(cristals >= cristalCost)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
