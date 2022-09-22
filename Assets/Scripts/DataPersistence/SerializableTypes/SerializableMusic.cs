[System.Serializable]
public class SerializableMusic
{
    public int id;
    public bool canPlay;
    public int score;
    public int star;

    public SerializableMusic(MusicSO musicSO)
    {
        SetMusic(musicSO);
    }

    public void SetMusic(MusicSO musicSO)
    {
        id = musicSO.id;
        canPlay = musicSO.canPlay;
        score = musicSO.score;
        star = musicSO.star;
    }
}