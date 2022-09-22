using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWithAudio : MonoBehaviour
{
    private bool canSpawn = true;
    public GameObject spawnedGO, obsPrefab;
    public Material goodMat, badMat;
    private Transform playerPos;
    private AudioPeer audioPeer;
    private int obsIndex = 2;

    private void Start()
    {
        audioPeer = GetComponent<AudioPeer>();
        playerPos = GameObject.FindGameObjectWithTag(Tags.playerMovement).transform;
    }

    private void Update()
    {
        SpawnCube();
    }

    private void SpawnCube()
    {
        if (audioPeer.highestBand > .65f && audioPeer.highestBand2 > .45f && canSpawn)
        {
            canSpawn = false;
            spawnedGO = Instantiate(obsPrefab, new Vector3(0, GameManager.instance.ObsSpawnY, playerPos.position.z + GameManager.instance.spawnDistance), Quaternion.identity);

            spawnedGO.transform.SetParent(transform);
            int randomSayi = Random.Range(0, 100);
            if (randomSayi < 80)
            {
                // saða sola +1
                if (Random.Range(0, 2) == 0 ? true : false)
                {
                    obsIndex--;
                    if (obsIndex < 0)
                        obsIndex = 1;
                }
                else
                {
                    obsIndex++;
                    if (obsIndex > 4)
                        obsIndex = 3;
                }
            }
            spawnedGO.transform.GetChild(obsIndex).name = "good";

            List<Transform> goods = new List<Transform>();
            List<Transform> bads = new List<Transform>();

            foreach (Transform child in spawnedGO.transform)
            {
                if (child.name == "good")
                {
                    goods.Add(child);
                }
                else if (child.name == "bad")
                {
                    bads.Add(child);
                }
            }
            foreach (Transform item in goods)
            {
                item.GetComponent<Obstacle>().color = GameManager.instance.spawnColorType;
                item.GetChild(0).GetComponent<MeshRenderer>().material = goodMat;
            }
            foreach (Transform item in bads)
            {
                item.GetComponent<Obstacle>().color = GameManager.instance.antiColorType;
                item.GetChild(0).GetComponent<MeshRenderer>().material = badMat;
            }

            CreateEmptySpacesInObs(spawnedGO.transform);
            StartCoroutine(SpawnDelayCo());
        }
    }

    private void CreateEmptySpacesInObs(Transform obs)
    {
        switch (obsIndex)
        {
            case 0:
                obs.GetChild(1).gameObject.SetActive(false);
                break;

            case 1:
                obs.GetChild(2).gameObject.SetActive(false);
                break;

            case 2:
                if (Random.Range(0, 2) == 1)
                {
                    obs.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    obs.GetChild(3).gameObject.SetActive(false);
                }
                break;

            case 3:
                obs.GetChild(2).gameObject.SetActive(false);
                break;

            case 4:
                obs.GetChild(3).gameObject.SetActive(false);
                break;

            default:
                Debug.Log("Boyle bir index yok");
                break;
        }
    }

    private IEnumerator SpawnDelayCo()
    {
        yield return new WaitForSeconds(GameManager.instance.spawnDelay);
        canSpawn = true;
    }
}