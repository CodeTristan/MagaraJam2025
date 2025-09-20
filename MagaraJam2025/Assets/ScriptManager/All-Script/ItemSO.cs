using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemSO : ScriptableObject
{
    public ItemName ItemName;
    [TextArea(3, 10)]
    public string Description;
    public Sprite Sprite;
    public int MaxAmount;
    public int Cost;
}
