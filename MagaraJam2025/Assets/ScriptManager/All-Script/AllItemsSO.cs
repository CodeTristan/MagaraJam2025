using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "AllItems", menuName = "Game Data/AllItems", order = 2)]
public class AllItemsSO : ScriptableObject
{
    public List<ItemSO> Items;

    private Dictionary<ItemName, Item> allItemsDict;
    private Dictionary<ItemName, Item> inventoryItemsDict;


    public void Init()
    {
        allItemsDict = Items.ToDictionary(item => item.ItemName, item => new Item(item, 0));
        inventoryItemsDict = new Dictionary<ItemName, Item>();

        Debug.Log("AllItems initialized with " + Items.Count + " items.");
    }
    public void Init(ItemSaveData[] saveData)
    {
        Init();
        foreach (var data in saveData)
        {
            if (allItemsDict.TryGetValue(data.itemName, out var item))
            {
                item.Amount = data.amount;
            }
            else
            {
                Debug.LogError("Item not found in dictionary: " + data.itemName);
            }

            if (data.amount > 0)
            {
                inventoryItemsDict[data.itemName] = item; // Add to inventory if amount > 0
            }
        }
    }


    public Item GetItem(ItemName itemName)
    {
        if(allItemsDict.TryGetValue(itemName, out var item))
        {
            return item;
        }

        Debug.LogError("Item not found: " + itemName);
        return null;
    }

    public void AddItem(Item item)
    {
        if (allItemsDict.ContainsKey(item.Data.ItemName) && item.Data.MaxAmount >= allItemsDict[item.Data.ItemName].Amount + item.Amount)
        {
            allItemsDict[item.Data.ItemName].Amount += item.Amount;
        }
        else
        {
            allItemsDict[item.Data.ItemName] = item;
            allItemsDict[item.Data.ItemName].Amount = item.Amount;
            allItemsDict[item.Data.ItemName].HasBeenBought = true;
        }
    }

    public void RemoveItem(Item item)
    {
        if (allItemsDict.TryGetValue(item.Data.ItemName, out var existingItem))
        {
            existingItem.Amount -= item.Amount;
            if (existingItem.Amount <= 0)
            {
                allItemsDict.Remove(item.Data.ItemName);
                inventoryItemsDict.Remove(item.Data.ItemName);
            }
        }
        else
        {
            Debug.LogError("Item not found in inventory: " + item.Data.ItemName);
        }
    }

    public List<Item> GetInventoryItems()
    {
        return inventoryItemsDict.Values.ToList();
    }

    public Dictionary<ItemName, Item> GetAllItemsDict()
    {
        return allItemsDict;
    }

    public List<Item> GetAllItems()
    {
        return allItemsDict.Values.ToList();
    }

    public ItemSaveData[] GetSaveData()
    {
        return allItemsDict.Values.Select(item => new ItemSaveData(item)).ToArray();
    }
}
