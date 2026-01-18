using Coop.Interaction;
using Coop.Scene;
using Mirror;
using TMPro;
using UnityEngine;
using Zenject;

namespace Coop.Player
{
    public class PlayerInteraction : NetworkBehaviour
    {
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private float _interactDistance = 3f;
        [SerializeField] private LayerMask _interactLayer;

        private TMP_Text _interactionText;
        private IInteractable _currentTarget;
        private SceneScript _sceneScript;

        [Inject]
        public void Construct(SceneScript sceneScript) => _sceneScript = sceneScript;

        private void Update()
        {
            if (!isLocalPlayer) return;

            CheckForInteractable();

            if (Input.GetKeyDown(KeyCode.E) && _currentTarget != null)
            {
                var interactableComponent = _currentTarget as Component;
                if (interactableComponent)
                {
                    var targetIdentity = interactableComponent.GetComponentInParent<NetworkIdentity>();

                    if (targetIdentity)
                        CmdTryInteract(targetIdentity.gameObject);
                    else
                        Debug.LogWarning(
                            "Пытаемся взаимодействовать с объектом без NetworkIdentity! В сети это работать не будет.");
                }
            }
        }

        private void CheckForInteractable()
        {
            var ray = new Ray(_cameraRoot.position, _cameraRoot.forward);
            if (Physics.Raycast(ray, out var hit, _interactDistance, _interactLayer))
            {
                var interactable = hit.collider.GetComponent<IInteractable>() ??
                                   hit.collider.GetComponentInParent<IInteractable>();

                if (interactable != null)
                {
                    _currentTarget = interactable;

                    _sceneScript.SetInteractionText(true, interactable.InteractionPrompt);
                    return;
                }
            }

            _currentTarget = null;
            _sceneScript.SetInteractionText(false);
        }

        [Command]
        private void CmdTryInteract(GameObject targetObj)
        {
            if (!targetObj) return;

            var interactable = targetObj.GetComponent<IInteractable>() ??
                               targetObj.GetComponentInParent<IInteractable>();

            interactable?.OnInteract();
        }
    }
}