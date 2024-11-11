using Extensions;
using Player;
using UnityEngine;

namespace Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _interactTime = 1f;
        [SerializeField] private string _label;
        [SerializeField] private Collider _triggerArea;

        protected delegate void OnInteractCallback();

        protected OnInteractCallback OnInteractCallbacks;

        public float InteractTime => _interactTime;
        public string Label => _label;

        public void Interact()
        {
            OnInteractCallbacks();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponentIfIsTag("Player", out PlayerManager manager))
            {
                manager.AddInteractable(this);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponentIfIsTag("Player", out PlayerManager manager))
            {
                manager.RemoveInteractable(this);
            }
        }
    }
}
