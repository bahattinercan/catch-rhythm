using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MeshListSO")]
public class MeshListSO : ScriptableObject
{
    public List<Mesh> list;
}