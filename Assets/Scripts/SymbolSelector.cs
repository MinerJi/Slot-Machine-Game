using System.Collections.Generic;
using UnityEngine;


//Summary
//Selects a random symbol category based on predefined probability weights.
//Acts as a weighted lottery, where categories with higher weights have a 
//proportionally higher chance of being chosen.
//he string name of the selected category, or "NoCategory" if the list is empty.
public class SymbolSelector : MonoBehaviour
{
    public DataContainer container;

    public string Select()
    {
        int netWeight = 0, currentWeight = 0, randomSelect = 0;

        // Calculate total weight of all symbols
        foreach (var symbol in container.symbolDatas)
        {
            netWeight += symbol.Weight;
        }

        // Pick a random number from 0 to total weight (exclusive)
        // This decides which symbol will be selected
        randomSelect = Random.Range(0, netWeight);

        // Traverse again and keep adding weights cumulatively
        foreach (var symbol in container.symbolDatas)
        {
            currentWeight += symbol.Weight;
            // When cumulative weight crosses random number,
            // that symbol is the selected one
            if (currentWeight > randomSelect)
            {
                return symbol.Category;
            }
        }
        // Fallback case, only if something unexpected happens
        return "NoCategory";
    }

}
