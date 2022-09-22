using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<SerializableMusic> musics;
    public int stars;
    public int cristal;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        musics = new List<SerializableMusic>();
        stars = 0;
        cristal = 250;
    }
}