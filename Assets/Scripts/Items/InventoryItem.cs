using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory Item")]
    public class InventoryItem : Item
    {
        [SerializeField] private Sprite _image;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        
        public Sprite Image => _image; 
        public string Name => _name;
        public string Description => _description;
    }
}