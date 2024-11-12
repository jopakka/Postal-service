using System.Collections.Generic;
using System.Collections.ObjectModel;
using Items;
using UnityEngine;

namespace Inventory
{
    public abstract class Inventory : MonoBehaviour
    {
        [SerializeField, Min(1)] private int _inventoryCapacity = 10;
        
        [SerializeField] private List<InventoryItem> _items = new();
        
        public delegate void OnInventoryUpdated();
        public OnInventoryUpdated OnInventoryUpdatedCallback;
        
        public int InventoryCapacity => _inventoryCapacity;
        public ReadOnlyCollection<InventoryItem> Items => _items.AsReadOnly();

        public bool AddItem(InventoryItem item)
        {
            if (_items.Count >= InventoryCapacity || !item) return false;
            _items.Add(item);
            OnInventoryUpdatedCallback();
            return true;
        }

        public bool RemoveItem(InventoryItem item)
        {
            var removed = _items.Remove(item);
            if (removed) OnInventoryUpdatedCallback();
            return removed;
        }
    }
}