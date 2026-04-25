using System.Collections.Generic;
using TMPro;
using UnityEngine;
//summary
// Manages the money, handling betting, reward calculations, and message updates for transactions.

public class TransactionManager : MonoBehaviour
{
    public DataContainer container;
    public TextMeshProUGUI money,message;
    public int initialAmount;
    public Texture specialSymbol;
    public int minReward;

    // Maps each standard symbol texture to its corresponding payout multiplier
    private Dictionary<Texture, float> _rewardValue = new Dictionary<Texture, float>();
    private int _betAmount;
    // Initializes the player's starting balance and populates the reward dictionary
    void Start()
    {
        Application.targetFrameRate = 60;
       container.remainingAmount = initialAmount;
        money.text = container.remainingAmount.ToString();

        foreach (var reward in container.symbolDatas)
        {
            _rewardValue[reward.Symbol] = reward.Reward;
        }
    }
    // Attempts to deduct the specified bet amount from the player's balance.
    public void DeductMoney(int amount)
    {
       
        if (container.remainingAmount >= amount)
        {
            container.remainingAmount -= amount;
            money.text = container.remainingAmount.ToString();
            _betAmount = amount;
            message.text = "Spinning";
        }
    }
    // Evaluates the final symbols, calculates winnings, and updates the player's balance.
    public void CreditMoney(List<Texture> selectedTextures)
    {
        if (selectedTextures.Count == 0)
        {
            return;
        }

        int specialSymbolCount = -1, winningAmount = 0;
     
        for (int i = 0; i < selectedTextures.Count; i++)
        {
            if (selectedTextures[i] == specialSymbol)
            {
                specialSymbolCount++;
            }

        }
     
        if (specialSymbolCount > -1)
        {
            winningAmount = _betAmount * (minReward + specialSymbolCount);
            container.remainingAmount += winningAmount;
        }
        else
        {
            for (int i = 0; i < selectedTextures.Count - 1; i++)
            {
                if (selectedTextures[i] != selectedTextures[i + 1])
                {
                
                    message.text = "Try Again :(";
                    return;
                }
            }

            winningAmount = (int)(_betAmount * _rewardValue[selectedTextures[0]]);
            container.remainingAmount += winningAmount ;
            
        }
        message.text = "+" + winningAmount;
        money.text = container.remainingAmount.ToString();
       
    }   
}
