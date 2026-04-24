using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SymbbolData
{
    public Texture Symbol;
    public string Category;
    public int Weight;
    public float Reward;
}
public class DataContainer : MonoBehaviour
{
    public List<SymbbolData> symbolDatas;
}
