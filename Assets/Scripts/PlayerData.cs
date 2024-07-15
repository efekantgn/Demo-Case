using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private int coin = 0;
    public Action OnCoinChanged = null;
    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
            OnCoinChanged?.Invoke();
        }
    }
}

