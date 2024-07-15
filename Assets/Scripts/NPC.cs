using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public Shop Shop;
    PlayerController playerController;

    [HideInInspector] public Transform t { get => transform; }

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }
    public void OpenNPCMarket()
    {
        Shop.gameObject.SetActive(true);
        Shop.InitalizeInventory();
        playerController.EnableDisableInputActions(true);
    }
    public void CloseNPCShop()
    {
        Shop.gameObject.SetActive(false);
        playerController.EnableDisableInputActions(false);
    }
}
