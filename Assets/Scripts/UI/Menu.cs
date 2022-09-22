using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour, IDataPersistence
{
    public static Menu instance;

    public event Action<int> OnCristalChanged;

    [SerializeField] private MusicListSO musicListSO;
    private IntSO cristals;
    private IntSO stars;

    private Sprite closedLock, openedLock;

    private Transform musicButtonContainer;
    private GameObject musicButtonPrefab;
    private TextMeshProUGUI cristalText;
    private TextMeshProUGUI starText;
    private Color filledStarColor;

    public Button selectedButton;

    private void Awake()
    {
        instance = this;
        musicListSO = Resources.Load<MusicListSO>(typeof(MusicListSO).Name);
        musicButtonPrefab = Resources.Load<PrefabSO>("MusicButton_" + typeof(PrefabSO).Name).prefab;
        musicButtonContainer = GameObject.Find("musicButtonContainer").transform;
        filledStarColor = Resources.Load<ColorSO>("FilledStar_" + typeof(ColorSO).Name).color;
        cristals = Resources.Load<IntSO>("PlayerCristals_" + typeof(IntSO).Name);
        stars = Resources.Load<IntSO>("PlayerStars_" + typeof(IntSO).Name);
        closedLock= Resources.Load<Sprite>("ClosedLock_"+typeof(Sprite).Name);
        openedLock = Resources.Load<Sprite>("OpenedLock_" + typeof(Sprite).Name);
    }

    // Start is called before the first frame update
    private void Start()
    {
        cristalText = GameObject.Find("cristalHolder/text").GetComponent<TextMeshProUGUI>();
        starText = GameObject.Find("starHolder/text").GetComponent<TextMeshProUGUI>();
        GameObject.Find("randomMusicButton").GetComponent<Button>().onClick.AddListener(RandomPlayButton);
    }

    public void SetMusicPanel(MusicSO music, Transform musicPanel)
    {
        musicPanel.SetParent(musicButtonContainer);
        musicPanel.localScale = Vector3.one;
        musicPanel.Find("image").GetComponent<Image>().sprite = music.image;
        string[] musicStringList = music.clip.name.Split('-'); // 0. id, 1. music name, 2. artist name

        musicPanel.Find("panel/musicNamePanel/text").GetComponent<TextMeshProUGUI>().text = musicStringList[1];
        musicPanel.Find("panel/singerNamePanel/text").GetComponent<TextMeshProUGUI>().text = musicStringList[2];
        
        TimeSpan time = TimeSpan.FromSeconds(music.clip.length);
        musicPanel.Find("panel/musicTimePanel/text").GetComponent<TextMeshProUGUI>().text = time.ToString(@"m\:ss");

        // stars
        List<Image> starImages = new List<Image>();
        foreach (Transform child in musicPanel.Find("panel/starImages"))
        {
            if (child.name == "star")
                starImages.Add(child.GetComponent<Image>());
        }
        for (int j = 0; j < music.star; j++)
        {
            starImages[j].color = filledStarColor;
        }

        int price = (int)music.clip.length * 4;

        Transform cristalButtonT = musicPanel.Find("panel/buttonsPanel/cristalButton");
        cristalButtonT.name = "cristalButton-" + music.id;
        cristalButtonT.GetComponent<MusicCristalButtonUI>().SetCristalCost(price);
        cristalButtonT.GetComponent<MenuMusicButtonUI>().SetCristalCost(price);
        cristalButtonT.Find("text").GetComponent<TextMeshProUGUI>().text = price.ToString();

        musicPanel.Find("panel/buttonsPanel/reklamButton").name = "reklamButton-" + music.id;
        musicPanel.Find("panel/buttonsPanel/playButton").name = "playButton-" + music.id;

        // panel buttons
        MusicPanelButtons(music, musicPanel,price);
    }

    public void MusicPanelButtons(MusicSO music, Transform musicPanel,int price)
    {
        Button cristalButton = musicPanel.Find("panel/buttonsPanel").GetChild(0).GetComponent<Button>();
        Button reklamButton = musicPanel.Find("panel/buttonsPanel").GetChild(1).GetComponent<Button>();
        Button playButton = musicPanel.Find("panel/buttonsPanel").GetChild(2).GetComponent<Button>();

        Image lockImage = musicPanel.Find("panel/detailPanel/lockImage").GetComponent<Image>();               

        // if music bought, play button will be visible
        if (music.canPlay)
        {
            cristalButton.gameObject.SetActive(false);
            reklamButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
            lockImage.sprite = openedLock;
        }
        // else cristal and ad(reklam) button will be visible
        else
        {
            cristalButton.gameObject.SetActive(true);
            if (cristals.value >= price)
            {
                cristalButton.interactable = true;
            }
            else
            {
                cristalButton.interactable = false;
            }
            reklamButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive(false);
            lockImage.sprite = closedLock;
        }
    }

    public void CristalButton(int id, int cristalCost, Transform musicPanel)
    {
        cristals.value -= cristalCost;
        cristalText.text = cristals.value.ToString();
        OnCristalChanged?.Invoke(cristals.value);
        musicListSO.musics[id].canPlay = true;
        MusicPanelButtons(musicListSO.musics[id], musicPanel, cristalCost);
    }

    public void ReklamButton(int id)
    {
    }

    public void PlayButton(int id)
    {
        PlayerPrefs.SetInt(Prefs.selectedMusicIndex, id);
        SceneManager.LoadScene(SceneList.gameplay);
        DataPersistenceManager.instance.SaveGame();
    }

    public void RandomPlayButton()
    {
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

    #region IDataPersistance

    public void LoadData(GameData data)
    {
        // load music datas
        MusicListSO musicListSO = Resources.Load<MusicListSO>(typeof(MusicListSO).Name);
        for (int i = 0; i < data.musics.Count; i++)
        {
            musicListSO.musics[i].SetMusic(data.musics[i]);
        }

        // cristal and star texts
        cristals.value = data.cristal;
        stars.value = data.stars;
        cristalText.text = cristals.value.ToString();
        starText.text = stars.value.ToString();

        // create music panel
        for (int i = 0; i < musicListSO.musics.Count; i++)
        {
            MusicSO music = musicListSO.musics[i];
            Transform musicPanel = Instantiate(musicButtonPrefab).transform;
            SetMusicPanel(music, musicPanel);
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
        data.cristal = cristals.value;
        data.stars = stars.value;
        cristalText.text = cristals.value.ToString();
        starText.text = stars.value.ToString();
    }

    #endregion IDataPersistance
}