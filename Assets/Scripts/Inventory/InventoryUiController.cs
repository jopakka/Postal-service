using UnityEngine;

namespace Inventory
{
    public class InventoryUiController : MonoBehaviour
    {
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private InventoryUiItem _itemPrefab;

        private Inventory _inventoryController;

        private void Start()
        {
            InstantiateInventoryItems(150);
        }

        public void OpenInventory(Inventory inventory)
        {
            _inventoryController = inventory;
            UpdateInventory();
            _inventoryController.OnInventoryUpdatedCallback += UpdateInventory;
            gameObject.SetActive(true);
        }

        public void CloseInventory()
        {
            _inventoryController.OnInventoryUpdatedCallback -= UpdateInventory;
            gameObject.SetActive(false);
        }
        
        private void UpdateInventory() 
        {
            UpdateInventorySize(_inventoryController.InventoryCapacity);
            UpdateInventoryImages();
        }

        private void UpdateInventorySize(int size)
        {
            for (var i = 0; i < _itemContainer.childCount; i++)
            {
                var item = _itemContainer.GetChild(i).GetComponent<InventoryUiItem>();
                item.gameObject.SetActive(i < size);
                item.SetItem(null);
            }
        }

        private void UpdateInventoryImages()
        {
            for (var i = 0; i < _inventoryController.Items.Count; i++)
            {
                var item = _inventoryController.Items[i];
                var position = _itemContainer.GetChild(i).GetComponent<InventoryUiItem>();
                position.SetItem(item);
            }
        }

        private void InstantiateInventoryItems(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Instantiate(_itemPrefab, _itemContainer);
            }
        }
    }
}