using System.Collections.Generic;
using UnityEngine;

//summary
//Provides a shared data container accessible across multiple scripts
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
    [HideInInspector]
    public int remainingAmount,betAmount;
}
