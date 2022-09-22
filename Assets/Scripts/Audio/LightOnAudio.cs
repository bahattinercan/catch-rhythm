using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightOnAudio : MonoBehaviour
{
    public int band;
    public float minIntensity, maxIntensity;
    private Light light;

    // Start is called before the first frame update
    private void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    private void Update()
    {
        //light.intensity = (AudioPeer.audioBandBuffer[band] * (maxIntensity - minIntensity)) + minIntensity;
    }
}