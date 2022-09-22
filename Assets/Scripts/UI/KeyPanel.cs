using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPanel : MonoBehaviour
{
    private List<Image> images;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        images = new List<Image>();
        for (int i = 0; i < transform.childCount; i++)
        {
            images.Add(transform.GetChild(i).GetComponent<Image>());
        }
        UpdateKeys(0);
    }

    public void UpdateKeys(int keys)
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < keys; i++)
        {
            images[i].color = new Color(1, 1, 1, 1);
        }
    }
}