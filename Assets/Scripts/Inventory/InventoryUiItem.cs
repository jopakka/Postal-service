using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryUiItem : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private static readonly Color DefaultColor = new(118f / 255f, 118f / 255f, 118f / 255f);
        
        public void SetItem(InventoryItem item)
        {
            if (item)
            {
                _image.sprite = item.Image;
                _image.color = Color.white;
            }
            else
            {
                _image.sprite = null;
                _image.color = DefaultColor;
            }
        }
    }
}