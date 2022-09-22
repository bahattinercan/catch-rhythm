using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    private AudioSource audioSource;
    private float[] samples = new float[512];
    private float[] freqBands = new float[8];
    private float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];
    public float[] freqBandHighest = new float[8];
    public float[] audioBand = new float[8];
    public float[] audioBandBuffer = new float[8];
    public float audioProfileFloat = 5; // new

    // spawn variables
    public float highestBand = 0, highestBand2 = 0;

    private float sampleMultiplier;
    public bool playOnStart = false;
    public bool isStop = false;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.instance.OnGamePaused += GamePaused;
        GameManager.instance.OnGameResumed += GameResumed;
        AudioProfile(audioProfileFloat); // new
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GameManager.instance.musicSO.clip;
        if (playOnStart)
            audioSource.Play();

        sampleMultiplier = Mathf.Pow(audioSource.volume, -1);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isStop)
            audioSource.Stop();
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        CheckForHighestBand();
    }

    private void GamePaused(object sender, System.EventArgs e)
    {
        audioSource.Pause();
    }

    public void GameResumed(object sender, System.EventArgs e)
    {
        audioSource.UnPause();
    }

    // new
    private void AudioProfile(float value)
    {
        for (int i = 0; i < 8; i++)
        {
            freqBandHighest[i] = value;
        }
    }

    private void CheckForHighestBand()
    {
        highestBand = 0;
        for (int i = 0; i < 8; i++)
        {
            float band = audioBand[i];
            if (band > highestBand)
            {
                highestBand2 = highestBand;
                highestBand = band;
            }
            else if (band < highestBand && band > highestBand2)
            {
                highestBand2 = band;
            }
        }
    }

    private void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBands[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBands[i];
            }
            audioBand[i] = (freqBands[i] / freqBandHighest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
        }
    }

    private void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] *= sampleMultiplier;
        }
    }

    private void BandBuffer()
    {
        for (int g = 0; g < 8; g++)
        {
            if (freqBands[g] > bandBuffer[g])
            {
                bandBuffer[g] = freqBands[g];

                bufferDecrease[g] = .002f;
            }
            if (freqBands[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1.2f;
            }
        }
    }

    private void MakeFrequencyBands()
    {
        /*
         * 22050 / 512 = 43 hertz per sample
         *
         * 0-2 = 86 hertz
         * 1-4 = 172 hertz 87-258
         * 2-8 = 344 hertz 259-602
         * 3-16 = 688 hertz 603-1290
         * 4-32 = 1376 hertz 1291-2666
         * 5-64 = 2572 hertz 2667-5418
         * 6-128 = 5504 hertz 5419-10922
         * 7-256 = 11008 hertz 10923-21930
         */

        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
                sampleCount += 2;

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;
            freqBands[i] = average * 10;
        }
    }
}