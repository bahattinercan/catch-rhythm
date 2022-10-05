using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    #region Variables

    public static GameManager instance;

    public event EventHandler OnGamePaused;

    public event EventHandler OnGameResumed;

    public event Action<int> OnGameFinished;

    public event Action<int> OnScoreChanged;

    public event Action<int> OnHpChanged;

    public event Action<int> OnKeyChanged;

    // scriptable objects
    private MeshListSO playerMeshList;

    private PrefabSO colorChangePrefabSO;
    public MusicSO musicSO;
    private MaterialsSO materials;
    private PrefabListSO obsPrefabList;
    private AudioClipSO collectSound;

    // references
    public Transform player, playerMovement;

    private AudioSource audioSource;
    private BackgroundColorChanger backgroundColorChanger;
    private SpawnerWithAudio spawnerWithAudio;
    private ParticleSystem collectParticle;

    // variables
    private int score;

    private int maxGoal = 1000; // maxGoal * .4 -> 1 star | .6 -> 2 star | 1 -> 3 star
    private int[] goals = new int[3];
    private int key;
    private int heath = 2;
    private float obsSpawnY = -3.5f;
    private float bgCubeScaleMultiplier = 3;
    public float spawnDistance = 92.85f;

    // game setting variables
    private float changeColorTime = 15f;

    public float obsRotationAngle = 40;
    public float spawnDelay;
    public float theLowestSpawnDelay;
    public float changeSpawnDelayValue;
    public float changeSpawnDelayTime = 30f;
    public int obsValue;
    public int cristalValue = 50;
    private int scoreToCristalDivider = 10;
    private int startStarNumber;

    // enums
    public EGameState eGameState;

    public EColor spawnColorType, antiColorType;
    private EShape spawnShapeType;

    public float ObsSpawnY { get => obsSpawnY; private set => obsSpawnY = value; }
    public float BgCubeScaleMultiplier { get => bgCubeScaleMultiplier; private set => bgCubeScaleMultiplier = value; }

    #endregion Variables

    #region Base Fuctions

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        obsPrefabList = Resources.Load<PrefabListSO>("Obstacle_" + typeof(PrefabListSO).Name);
        colorChangePrefabSO = Resources.Load<PrefabSO>("ColorChange_" + typeof(PrefabSO).Name);
        materials = Resources.Load<MaterialsSO>(typeof(MaterialsSO).Name);
        collectSound = Resources.Load<AudioClipSO>("Collect_" + typeof(AudioClipSO).Name);
        playerMeshList = Resources.Load<MeshListSO>("Player_" + typeof(MeshListSO).Name);
        AwakeGameplaySettingsSetup();
        AwakeMusicSetup();
        CalculateGoals();
        CalculateEndCristalValue();
    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        playerMovement = player.parent;
        eGameState = EGameState.play;
        backgroundColorChanger = GameObject.Find("backgroundMusicVirtualization").GetComponent<BackgroundColorChanger>();
        spawnerWithAudio = GameObject.FindGameObjectWithTag(Tags.audioSpawner).GetComponent<SpawnerWithAudio>();
        collectParticle = playerMovement.Find("collectParticle").GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        spawnColorType = GetRandomColorType();
        spawnShapeType = GetRandomShapeType();
        ChangeGameTheme(spawnColorType, spawnShapeType);

        score = 0;
        OnHpChanged?.Invoke(heath);
        OnScoreChanged?.Invoke(score);


        InvokeRepeating("DecreaseTheSpawnDelay", changeSpawnDelayTime, changeSpawnDelayTime);
        InvokeRepeating("CheckAndSpawnChangeColor", 0, changeColorTime);
    }

    private void AwakeGameplaySettingsSetup()
    {
        GameplaySettingsSO settings = Resources.Load<GameplaySettingsSO>("Normal_" + typeof(GameplaySettingsSO).Name);

        changeColorTime = settings.changeColorTime;
        obsRotationAngle = settings.obsRotationAngle;
        spawnDelay = settings.spawnDelay;
        theLowestSpawnDelay = settings.theLowestSpawnDelay;
        changeSpawnDelayTime = settings.changeSpawnDelayTime;
        changeSpawnDelayValue = settings.changeSpawnDelayValue;
        obsValue = settings.obsValue;
        cristalValue = settings.cristalValue;
        scoreToCristalDivider = settings.scoreToCristalDivider;
    }

    private void AwakeMusicSetup()
    {
        MusicListSO musicListSO = Resources.Load<MusicListSO>(typeof(MusicListSO).Name);

        if (!PlayerPrefs.HasKey(Prefs.selectedMusicIndex))
            PlayerPrefs.SetInt(Prefs.selectedMusicIndex, 0);

        musicListSO.SetClip(PlayerPrefs.GetInt(Prefs.selectedMusicIndex));

        musicSO = musicListSO.GetClip();
        startStarNumber = musicSO.star;
    }

    #endregion Base Fuctions

    #region Other Functions

    public int GetMaxGoal()
    {
        return maxGoal;
    }

    private void CalculateEndCristalValue()
    {
        float musicTime = musicSO.clip.length;
        cristalValue = (int)(musicTime * .375f);
    }

    public int GetCristalValue()
    {         
        return score / scoreToCristalDivider;
    }

    private void CalculateGoals()
    {
        float musicTime=musicSO.clip.length;
        maxGoal=(int)(musicTime*15f);
        goals[0] = (maxGoal * 4) / 10;
        goals[1] = (maxGoal * 6) / 10;
        goals[2] = maxGoal;
    }

    public int GetScoreStar()
    {
        int star = 0;
        for (int i = 0; i < goals.Length; i++)
        {
            if (score > goals[i])
                star = i + 1;
        }
        return star;
    }

    public void AddScore(int value, bool playParticle = true, bool playSound = false)
    {
        score += value;
        UpdateScore();
        if (playParticle)
            PlayCollectParticle();
        if (playSound)
            PlayCollectSound();
    }

    public void UpdateScore()
    {
        OnScoreChanged?.Invoke(score);
    }

    public void UpdateKey()
    {
        OnKeyChanged?.Invoke(key);
    }

    public void PlayCollectParticle()
    {
        collectParticle.Play();
    }

    public void PlayCollectSound()
    {
        audioSource.PlayOneShot(collectSound.clip);
    }

    private void DecreaseTheSpawnDelay()
    {
        spawnDelay -= changeSpawnDelayValue;
        if (spawnDelay < theLowestSpawnDelay)
            spawnDelay = theLowestSpawnDelay;
    }

    #endregion Other Functions

    #region Color & Shape Functions

    public EColor GetRandomColorType()
    {
        return (EColor)UnityEngine.Random.Range(0, 4);
    }

    public EShape GetRandomShapeType()
    {
        return (EShape)UnityEngine.Random.Range(0, 4);
    }

    private void CheckAndSpawnChangeColor()
    {
        // get random spawnColorType
        EColor randomColorType = spawnColorType;
        do
        {
            randomColorType = GetRandomColorType();
        } while (spawnColorType == randomColorType);
        spawnColorType = randomColorType;

        // get random shapeType
        EShape randomShapeType = spawnShapeType;
        do
        {
            randomShapeType = GetRandomShapeType();
        } while (spawnShapeType == randomShapeType);
        spawnShapeType = randomShapeType;

        // spawn color changer
        GameObject colorchangerGO = Instantiate(colorChangePrefabSO.prefab,
            new Vector3(0, ObsSpawnY, playerMovement.position.z + spawnDistance),
            Quaternion.identity);

        Color color = materials.GetMaterial(spawnColorType).color;
        ChangeColorObs changeColorObs = colorchangerGO.transform.GetChild(0).GetChild(0).GetComponent<ChangeColorObs>();
        changeColorObs.GetComponent<MeshRenderer>().materials[1].color = color;
        changeColorObs.eColor = spawnColorType;
        changeColorObs.eShape = spawnShapeType;

        ChangeGameTheme();
    }

    public void ChangePlayerTheme()
    {
        ChangePlayerTheme(spawnColorType, spawnShapeType);
    }

    public void ChangePlayerTheme(EColor colorType, EShape shapeType)
    {
        Transform playerMesh = playerMovement.Find("mesh");

        // color
        Color newColor = materials.GetMaterial(colorType).color;
        player.GetComponent<PlayerColor>().color = colorType;
        playerMesh.GetComponent<MeshRenderer>().materials[0].color = newColor;
        
        var backParticleMain = playerMesh.Find("backParticle").GetComponent<ParticleSystem>().main;
        backParticleMain.startColor = newColor;
        var collectMain = collectParticle.main;
        collectMain.startColor = newColor;

        //backgroundColorChanger.ChangeColor(newColor);

        // shape
        playerMesh.GetComponent<MeshFilter>().mesh = playerMeshList.list[(int)shapeType];
        var backParticleRenderer = playerMesh.Find("backParticle").GetComponent<ParticleSystemRenderer>();
        backParticleRenderer.mesh = playerMeshList.list[(int)shapeType];
        backgroundColorChanger.ChangeMesh(shapeType);
    }

    public void ChangeGameTheme()
    {
        ChangeGameTheme(spawnColorType, spawnShapeType);
    }

    public void ChangeGameTheme(EColor colorType, EShape shapeType)
    {
        // color
        antiColorType = colorType;
        do
        {
            antiColorType = GetRandomColorType();
        } while (spawnColorType == antiColorType);

        // spawnlanacak küplerin rengini deðiþtir
        spawnerWithAudio.goodMat = materials.GetMaterial(spawnColorType);
        spawnerWithAudio.badMat = materials.GetMaterial(antiColorType);

        // shape
        spawnerWithAudio.obsPrefab = obsPrefabList.list[(int)shapeType].prefab;
    }

    #endregion Color & Shape Functions

    #region Game Functions

    public void SaveScore()
    {
        if (musicSO.score < score)
        {
            MusicListSO musicListSO = Resources.Load<MusicListSO>(typeof(MusicListSO).Name);
            musicListSO.musics[musicSO.id].score = score;
            musicListSO.musics[musicSO.id].star = GetScoreStar();
        }
    }

    public void PauseGame()
    {
        eGameState = EGameState.pause;
        Time.timeScale = 0;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void ResumeGame()
    {
        eGameState = EGameState.play;
        Time.timeScale = 1;
        OnGameResumed?.Invoke(this, EventArgs.Empty);
    }

    public void RandomStartGame()
    {
        Time.timeScale = 1;

        // satýn alýnmýþ þarkýlar içinden random þarký seç ver selected music indexe ekle
        MusicListSO musicListSO = Resources.Load<MusicListSO>(typeof(MusicListSO).Name);
        List<MusicSO> boughtMusics = new List<MusicSO>();
        foreach (MusicSO music in musicListSO.musics)
        {
            if (music.canPlay == true)
            {
                // this music bought
                boughtMusics.Add(music);
            }
        }
        if (boughtMusics.Count == 1)
        {
            PlayerPrefs.SetInt(Prefs.selectedMusicIndex, boughtMusics[0].id);
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadScene(SceneList.gameplay);
        }
        else if (boughtMusics.Count > 1)
        {
            PlayerPrefs.SetInt(Prefs.selectedMusicIndex, boughtMusics[UnityEngine.Random.Range(0, boughtMusics.Count)].id);
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadScene(SceneList.gameplay);
        }
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        eGameState = EGameState.gameover;
        SaveScore();
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(SceneList.menu);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt(Prefs.selectedMusicIndex, musicSO.id);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Fail()
    {
        --heath;
        OnHpChanged?.Invoke(heath);
        if (heath < 0)
        {
            heath = 0;
            OnHpChanged?.Invoke(heath);
            GameOver();
        }
    }

    public void GameOver()
    {
        PauseGame();
        eGameState = EGameState.gameover;
        SaveScore();
        OnGameFinished?.Invoke(score);
    }

    public void Win()
    {
        PauseGame();
        eGameState = EGameState.win;
        SaveScore();
        OnGameFinished?.Invoke(score);
    }

    public void LoadData(GameData data)
    {
        MusicListSO musicListSO = Resources.Load<MusicListSO>(typeof(MusicListSO).Name);
        for (int i = 0; i < data.musics.Count; i++)
        {
            musicListSO.musics[i].SetMusic(data.musics[i]);
        }
    }

    public void SaveData(GameData data)
    {
        MusicListSO musicListSO = Resources.Load<MusicListSO>(typeof(MusicListSO).Name);
        if (data.musics.Count == 0)
        {
            for (int i = 0; i < musicListSO.musics.Count; i++)
            {
                SerializableMusic music = musicListSO.musics[i].GetSerializable();
                if (!data.musics.Contains(music))
                {
                    data.musics.Add(music);
                }
            }
        }
        else
        {
            for (int i = 0; i < musicListSO.musics.Count; i++)
            {
                data.musics[i] = musicListSO.musics[i].GetSerializable();
            }
        }
        data.cristal += GetCristalValue();
        if (startStarNumber < musicSO.star)
        {
            data.stars -= startStarNumber;
            data.stars += musicSO.star;
        }
    }

    #endregion Game Functions
}