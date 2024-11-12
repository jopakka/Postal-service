using System;
using System.Collections;
using System.Collections.Generic;
using Interactables;
using Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovementManager _movementManager;
        [SerializeField] private HudManager _hudManager;
        [SerializeField] private Inventory.Inventory _inventory;

        public Inventory.Inventory PlayerInventory
        {
            get => _inventory;
            set
            {
                _inventory = value;
                _hudManager.OpenInventory(_inventory);
            }
        }

        private InputAction _interactAction;
        private InputAction _inventoryAction;
        private IEnumerator _interactCoroutine;
        private bool _interactWaitComplete;

        private readonly List<Interactable> _interactables = new();

        private void Start()
        {
            _interactAction = InputSystem.actions.FindAction("Interact");
            _interactAction.performed += StartInteraction;
            _interactAction.canceled += CancelInteraction;
            
            _inventoryAction = InputSystem.actions.FindAction("Inventory");
            _inventoryAction.performed += ToggleInventory;
        }

        private void OnEnable()
        {
            _interactAction?.Enable();
            _inventoryAction?.Enable();
        }

        private void OnDisable()
        {
            _interactAction?.Disable();
            _inventoryAction?.Disable();
        }

        // Update is called once per frame
        private void Update()
        {
            HandleHud();
        }

        private void ToggleInventory(InputAction.CallbackContext context)
        {
            if (_hudManager.IsInventoryOpen)
            {
                _hudManager.CloseInventory();
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
            else
            {
                _hudManager.OpenInventory(_inventory);
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
        }

        private void StartInteraction(InputAction.CallbackContext context)
        {
            if (_interactCoroutine != null || !CanInteract(out var interactable)) return;
            _interactCoroutine = InteractCoroutine(interactable);
            StartCoroutine(_interactCoroutine);
        }

        private void CancelInteraction(InputAction.CallbackContext context)
        {
            CancelInteraction();
        }

        private void CancelInteraction()
        {
            if (_interactWaitComplete || _interactCoroutine == null) return;
            StopCoroutine(_interactCoroutine);
            _hudManager.ClearProgress();
            _interactCoroutine = null;
        }

        private IEnumerator InteractCoroutine(Interactable interactable)
        {
            _interactWaitComplete = false;
            var timer = 0f;
            while (timer < interactable.InteractTime)
            {
                timer += Time.deltaTime;
                _hudManager.SetProgress(timer / interactable.InteractTime);
                yield return new WaitForEndOfFrame();
            }

            _interactWaitComplete = true;
            _interactables.RemoveAt(0);
            interactable.Interact();
            _hudManager.ClearProgress();
            _interactWaitComplete = false;
            _interactCoroutine = null;

            yield return null;
        }

        private void HandleHud()
        {
            _hudManager.SetSpeed(_movementManager.Speed);
            if (CanInteract(out var interactable))
            {
                var actionLabel = _interactAction.GetBindingDisplayString(0);
                _hudManager.SetInteraction(actionLabel, interactable.Label);
            }
            else
            {
                _hudManager.ClearInteraction();
            }
        }

        private bool CanInteract(out Interactable interactable)
        {
            var canInteract = _interactables.Count > 0;
            interactable = canInteract ? _interactables[0] : null;

            return canInteract;
        }

        public bool AddInteractable(Interactable interactable)
        {
            if (_interactables.Contains(interactable)) return false;
            _interactables.Add(interactable);
            return true;

        }

        public bool RemoveInteractable(Interactable interactable)
        {
            if (!_interactables.Contains(interactable)) return false;
            _interactables.Remove(interactable);
            
            if (_interactables.Count == 0) CancelInteraction();
            
            return true;

        }
    }
}
