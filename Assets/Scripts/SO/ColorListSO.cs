using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ColorListSO")]
public class ColorListSO : ScriptableObject
{
    public List<ColorSO> colors1;
    public List<ColorSO> colors2;
    public List<ColorSO> colors3;

    public Color GetColor(EColor eColor, byte index)
    {
        Color color = new Color(1, 1, 1);
        switch (eColor)
        {
            case EColor.blue:
                color = colors1[index].color;
                break;

            case EColor.purple:
                color = colors2[index].color;
                break;

            case EColor.red:
                color = colors3[index].color;
                break;

            case EColor.yellow:
                color = colors3[index].color;
                break;

            default:
                Debug.Log("color bulunamadý");
                break;
        }
        return color;
    }

    public Color GetColor(byte colorNo, byte index)
    {
        Color color = new Color(1, 1, 1);
        switch (colorNo)
        {
            case 1:
                color = colors1[index].color;
                break;

            case 2:
                color = colors2[index].color;
                break;

            case 3:
                color = colors3[index].color;
                break;

            default:
                Debug.Log("color bulunamadý");
                break;
        }
        return color;
    }
}