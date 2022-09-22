using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MusicListSO")]
public class MusicListSO : ScriptableObject
{
    public List<MusicSO> musics;
    private MusicSO currentMusic;
    public int clipIndex;

    public void SetRandomClip()
    {
        clipIndex = Random.Range(0, musics.Count);
        currentMusic = musics[clipIndex];
    }

    public void SetClip(int index)
    {
        clipIndex = index;
        currentMusic = musics[clipIndex];
    }

    public MusicSO GetClip(int index)
    {
        clipIndex = index;
        currentMusic = musics[clipIndex];
        return currentMusic;
    }

    public MusicSO GetClip()
    {
        currentMusic = musics[clipIndex];
        return currentMusic;
    }

    public List<SerializableMusic> GetSerializableMusics()
    {
        List<SerializableMusic> musics = new List<SerializableMusic>();
        for (int i = 0; i < this.musics.Count; i++)
        {
            musics.Add(new SerializableMusic(this.musics[i]));
        }
        return musics;
    }
}