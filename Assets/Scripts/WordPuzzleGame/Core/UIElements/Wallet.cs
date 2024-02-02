using System;
using TMPro;
using UnityEngine;

namespace STGames
{
    public class Wallet : BaseBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldAmountText;

        private void Awake()
        {
            LS.RegisterListenerOn(SK.GoldAmount, OnGoldAmountChanged);
        }

        private void OnEnable()
        {
            OnGoldAmountChanged(LSh.CurrentGoldAmount);
        }

        private void OnGoldAmountChanged(int goldAmount)
        {
            goldAmountText.SetText(goldAmount.ToString());
        }
    }
}