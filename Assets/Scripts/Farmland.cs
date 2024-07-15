using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmland : MonoBehaviour, IInteractable
{
    public List<Seed> seeds = new List<Seed>();

    [HideInInspector] public Transform t => transform;

    public void PlantSeed(InventoryItemSO itemSO, Vector3 plantingPos)
    {
        Seed seed1 = Instantiate(itemSO.itemPrefab).GetComponent<Seed>();
        seed1.transform.position = plantingPos;
        seed1.plantedFarm = this;
        seed1.GetComponent<Rigidbody>().isKinematic = true;
        seed1.BeginGrow();
        seeds.Add(seed1);
    }

}
