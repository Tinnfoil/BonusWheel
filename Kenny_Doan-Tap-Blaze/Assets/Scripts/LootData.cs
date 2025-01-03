using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LootData
{
    public uint ID;
    public float Weight;
    [HideInInspector] public float ChanceToSpawn;
    public PrizeData PrizeData;
}

