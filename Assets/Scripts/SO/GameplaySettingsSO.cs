
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameplaySettingsSO")]
public class GameplaySettingsSO:ScriptableObject
{
    public float spawnDelay = .399f;
    public float theLowestSpawnDelay = .299f;
    public float changeSpawnDelayValue = .0125f;
    public float changeSpawnDelayTime = 15f;
    public float obsRotationAngle = 30;
    public int obsValue = 4;
    public int cristalValue = 30;
    public float changeColorTime = 15f;
    public int scoreToCristalDivider = 10;
}
