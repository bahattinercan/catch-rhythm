using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MusicSO")]
public class MusicSO : ScriptableObject
{
    public int id;
    public bool canPlay;
    public AudioClip clip;
    public int score;
    public int star;
    public Sprite image;

    /// <summary>
    /// 1 easy, 2 normal, 3 hard, 4 veteran
    /// </summary>
    public byte easyLevel = 1;

    // image, music name, music artist name, stars, isLocked, easy level, music price
    public void SetMusic(SerializableMusic music)
    {
        canPlay = music.canPlay;
        score = music.score;
        star = music.star;
    }

    public SerializableMusic GetSerializable()
    {
        return new SerializableMusic(this);
    }
}