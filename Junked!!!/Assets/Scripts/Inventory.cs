using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public List<Trash> inventorySlots { get; private set; } = new List<Trash>();


    public void AddItem(Trash item)
    {
        inventorySlots.Add(item);
    }
    
    public void RemoveItem(Trash item)
    {
        inventorySlots.Remove(item);
    }


}
