using System.Collections;
using UnityEngine;

public class CristalSpawner : MonoBehaviour
{
    private AudioSource audioSource;
    private PrefabSO cristalPrefabSO;

    private void Awake()
    {
        cristalPrefabSO = Resources.Load<PrefabSO>("Cristals_" + typeof(PrefabSO).Name);
    }

    private void Start()
    {
        audioSource = GameObject.Find("Music").GetComponent<AudioSource>();
        StartCoroutine(musicFinishCO(audioSource.GetComponent<MusicController>().startDelay));
    }

    private IEnumerator musicFinishCO(float startDelay)
    {
        yield return new WaitForSeconds(startDelay + .1f);
        while (true)
        {
            if (audioSource.isPlaying == false && GameManager.instance.eGameState == EGameState.play)
            {
                yield return new WaitForSeconds(.1f);
                Vector3 playerPos = GameManager.instance.player.transform.position;
                Vector3 spawnPos = new Vector3(0, GameManager.instance.ObsSpawnY + 1, playerPos.z + GameManager.instance.spawnDistance);
                Instantiate(cristalPrefabSO.prefab, spawnPos, Quaternion.identity);
                break;
            }
            yield return null;
        }
    }
}