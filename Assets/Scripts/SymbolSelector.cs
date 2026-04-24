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
        int netWeight=0, currentWeight=0, randomSelect=0;

        foreach (var symbol in container.symbolDatas)
        {
            netWeight += symbol.Weight;
        }
        randomSelect = Random.Range(0, netWeight);
        foreach (var symbol in container.symbolDatas)
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
