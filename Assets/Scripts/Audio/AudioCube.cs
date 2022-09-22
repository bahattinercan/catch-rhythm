using UnityEngine;

public class AudioCube : MonoBehaviour
{
    public int band;
    private float startScale = 1, scaleMultiplier = 15;
    public bool useBuffer;
    public Material material;
    public AudioPeer audioPeer;

    // Start is called before the first frame update
    private void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
        transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    private void Update()
    {
        if (useBuffer)
        {
            if (float.IsNaN(audioPeer.audioBandBuffer[band]))
                audioPeer.audioBandBuffer[band] = 0.01f;
            transform.localScale = new Vector3(transform.localScale.x, (audioPeer.audioBandBuffer[band] * scaleMultiplier) + startScale, transform.localScale.z);
            float audioFloat = audioPeer.audioBandBuffer[band] / 1.5f;
            Color color = new Color(audioFloat, audioFloat, audioFloat);
            material.SetColor("_EmissionColor", color);
        }
        else
        {
            if (float.IsNaN(audioPeer.audioBand[band]))
                audioPeer.audioBand[band] = 0.01f;
            transform.localScale = new Vector3(transform.localScale.x, (audioPeer.audioBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
            float audioFloat = audioPeer.audioBand[band] / 1.5f;
            Color color = new Color(audioFloat, audioFloat, audioFloat);
            material.SetColor("_EmissionColor", color);
        }
    }
}