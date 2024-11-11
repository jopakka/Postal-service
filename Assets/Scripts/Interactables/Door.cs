using UnityEngine;

namespace Interactables
{
    public class Door : Interactable
    {
        private void Start()
        {
            OnInteractCallbacks += DoorInteract;
        }

        private void DoorInteract()
        {
            Debug.Log("You entered home");
        }
    }
}
