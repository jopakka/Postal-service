using System.Collections;
using System.Collections.Generic;
using Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovementManager _movementManager;
        [SerializeField] private HudManager _hudManager;

        private InputAction _interactAction;
        private float _interactTimer;
        private bool _isInteractPressed;

        private readonly List<Interactable> _interactables = new();

        private void Start()
        {
            _interactAction = InputSystem.actions.FindAction("Interact");
        }

        // Update is called once per frame
        private void Update()
        {
            HandleHud();
            HandleInteract();
        }

        private void HandleInteract()
        {
            if (!CanInteract(out var interactable)) return;

            if (_interactAction.WasPerformedThisFrame())
            {
                _isInteractPressed = true;
                _interactTimer = 0f;
            }
            else if (_interactAction.WasCompletedThisFrame())
            {
                _isInteractPressed = false;
                _hudManager.ClearProgress();
            }

            if (!_isInteractPressed) return;
            
            _interactTimer += Time.deltaTime;
            _hudManager.SetProgress(_interactTimer / interactable.InteractTime);

            if (!(_interactTimer >= interactable.InteractTime)) return;

            StartCoroutine(Interact());
        }

        private IEnumerator Interact()
        {
            _isInteractPressed = false;
            var interactable = _interactables[0];
            _interactables.RemoveAt(0);
            interactable.Interact();
            _hudManager.ClearProgress();
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
            return true;

        }
    }
}
