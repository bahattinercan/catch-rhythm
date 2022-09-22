using System.Collections.Generic;
using UnityEngine;

public class AudioCubeScale : MonoBehaviour
{
    public int band;
    private float startScale = 12;
    public bool useBuffer;
    private List<Material> materials = new List<Material>();
    public AudioPeer audioPeer;

    // Start is called before the first frame update
    private void Start()
    {
        startScale = transform.localScale.x;
        foreach (Transform item in transform)
        {
            materials.Add(item.GetComponent<MeshRenderer>().material);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (useBuffer)
        {
            if (float.IsNaN(audioPeer.audioBandBuffer[band]))
                audioPeer.audioBandBuffer[band] = 0.01f;

            float audioScale = startScale - (audioPeer.audioBandBuffer[band] * GameManager.instance.BgCubeScaleMultiplier);
            transform.localScale = new Vector3(audioScale, audioScale, transform.localScale.z);
            float audioFloat = audioPeer.audioBandBuffer[band] / 15f;
            Color color = new Color(audioFloat, audioFloat, audioFloat);
            foreach (var mat in materials)
            {
                mat.SetColor("_EmissionColor", color);
            }
        }
        else
        {
            if (float.IsNaN(audioPeer.audioBand[band]))
                audioPeer.audioBand[band] = 0.01f;

            float audioScale = startScale - (audioPeer.audioBand[band] * GameManager.instance.BgCubeScaleMultiplier);
            transform.localScale = new Vector3(audioScale, audioScale, transform.localScale.z);
            float audioFloat = audioPeer.audioBand[band] / 15f;
            Color color = new Color(audioFloat, audioFloat, audioFloat);
            foreach (var mat in materials)
            {
                mat.SetColor("_EmissionColor", color);
            }
        }
    }
}