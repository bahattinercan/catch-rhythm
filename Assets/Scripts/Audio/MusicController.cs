using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public float startDelay; // 3f

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(StartMusicCo());
    }

    private IEnumerator StartMusicCo()
    {
        yield return new WaitForSeconds(startDelay);
        GetComponent<AudioSource>().Play();
    }
}