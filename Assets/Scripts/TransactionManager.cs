using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransactionManager : MonoBehaviour
{
    public DataContainer container;
    public TextMeshProUGUI money;
    public int initialAmount;

    private Dictionary<Texture, float> _rewardValue = new Dictionary<Texture, float>();
    private int _remainingAmount,_betAmount;
    void Start()
    {
        _remainingAmount = initialAmount;
        money.text = _remainingAmount.ToString();
        
        foreach (var reward in container.symbolDatas)
        {
            {
                _rewardValue[reward.Symbol] = reward.Reward;
            }
        }
    }
    public void DeductMoney(int amount)
    {
       
        if (_remainingAmount >= amount)
        {
            _remainingAmount -= amount;
            money.text = _remainingAmount.ToString();
            _betAmount = amount;

        }
    }
    public void CreditMoney(List<Texture> selectedTextures)
    {
        if (selectedTextures.Count == 0)
        {
            return;
        }
        for (int i = 0; i < selectedTextures.Count - 1; i++)
        {
            if (selectedTextures[i] != selectedTextures[i + 1])
            {
                return;
            }
        }
      
        _remainingAmount += (int)(_betAmount * _rewardValue[selectedTextures[0]]);
        money.text = _remainingAmount.ToString();    

    }
}
