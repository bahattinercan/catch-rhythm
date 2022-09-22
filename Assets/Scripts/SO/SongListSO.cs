using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SongListSO")]
public class SongListSO : ScriptableObject
{
    public List<AudioClip> clips;
    private AudioClip currentClip;
    public int clipIndex;

    public void SetRandomClip()
    {
        clipIndex = Random.Range(0, clips.Count);
        currentClip = clips[clipIndex];
    }

    public void SetClip(int index)
    {
        clipIndex = index;
        currentClip = clips[clipIndex];
    }

    public AudioClip GetClip(int index)
    {
        clipIndex = index;
        currentClip = clips[clipIndex];
        return currentClip;
    }

    public AudioClip GetClip()
    {
        currentClip = clips[clipIndex];
        return currentClip;
    }
}