using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CategoryWeight
{
    public string Category;
    public int Weight;
}

public class SymbolSelector : MonoBehaviour
{
    public List<CategoryWeight> SymbolCategories;
   
    public string Select()
    {
        int NetWeight=0, CurrentWeight=0, RandomSelect=0;

        foreach (var symbol in SymbolCategories)
        {
            NetWeight += symbol.Weight;
        }
        RandomSelect = Random.Range(0, NetWeight);
        foreach (var symbol in SymbolCategories)
        {
            CurrentWeight += symbol.Weight;
            if(CurrentWeight > RandomSelect)
            {
                Debug.Log(symbol.Category);
                return symbol.Category;
            }
        }

        return "NoCategory";
    }

}
