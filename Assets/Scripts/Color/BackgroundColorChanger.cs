using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour
{
    private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    private AudioCubeScale[] audioCubes;

    private void Start()
    {
        audioCubes = transform.GetComponentsInChildren<AudioCubeScale>();
        foreach (AudioCubeScale audioCube in audioCubes)
        {
            foreach (Transform shapeTransform in audioCube.transform)
            {
                meshRenderers.Add(shapeTransform.GetComponent<MeshRenderer>());
            }
        }
        GameManager.instance.ChangePlayerTheme();
    }

    public void ChangeColor(Color color)
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.color = color;
        }
    }

    public void ChangeMesh(EShape shapeType)
    {
        foreach (AudioCubeScale audioCube in audioCubes)
        {
            foreach (Transform shape in audioCube.transform)
            {
                shape.gameObject.SetActive(false);
                if (shape.name == shapeType.ToString() || shape.name == "backgroundPlane")
                {
                    shape.gameObject.SetActive(true);
                }
            }
        }
    }
}