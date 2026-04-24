using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CategoryWeight
{
    public string Category;
    public int Weight;
}

//Summary
//Selects a random symbol category based on predefined probability weights.
//Acts as a weighted lottery, where categories with higher weights have a 
//proportionally higher chance of being chosen.
//he string name of the selected category, or "NoCategory" if the list is empty.
public class SymbolSelector : MonoBehaviour
{
    public List<CategoryWeight> categoryWeights;
   
    public string Select()
    {
        int netWeight=0, currentWeight=0, randomSelect=0;

        foreach (var symbol in categoryWeights)
        {
            netWeight += symbol.Weight;
        }
        randomSelect = Random.Range(0, netWeight);
        foreach (var symbol in categoryWeights)
        {
            currentWeight += symbol.Weight;
            if(currentWeight > randomSelect)
            {
                return symbol.Category;
            }
        }

        return "NoCategory";
    }

}
