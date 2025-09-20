using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemSO Data;
    public int Amount;
    public bool HasBeenBought;

    public Item(ItemSO data, int amount)
    {
        Data = data;
        Amount = amount;
    }
}

[System.Serializable]
public class ItemSaveData
{
    public ItemName itemName;
    public int amount;
    public bool HasBeenBought;
    public ItemSaveData(Item item)
    {
        itemName = item.Data.ItemName;
        amount = item.Amount;
        HasBeenBought = item.HasBeenBought;
    }
}

public enum ItemName
{
}
