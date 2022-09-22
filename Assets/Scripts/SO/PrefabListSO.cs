using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PrefabListSO")]
public class PrefabListSO : ScriptableObject
{
    public List<PrefabSO> list;
}