using Items;
using UnityEngine;

namespace Inventory
{
    public class Backpack : Inventory
    {
        public InventoryItem _tempItem;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AddItem(_tempItem);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                RemoveItem(_tempItem);
            }
        }
    }
}