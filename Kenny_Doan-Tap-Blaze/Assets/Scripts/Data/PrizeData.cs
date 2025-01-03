using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PrizeData")]
public class PrizeData : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public GameObject Loot;
    public bool TimePrize;
    public float Amount;
}
