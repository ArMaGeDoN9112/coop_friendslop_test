using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Interaction.InteractableObjects
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class SceneSwitcher : NetworkBehaviour, IInteractable
    {
        [SerializeField] private string[] _sceneNames;
        [SerializeField] private InteractiveButton _interactiveButtonIncrease;
        [SerializeField] private InteractiveButton _interactiveButtonDecrease;
        private NetworkManager _networkManager;

        [SyncVar(hook = nameof(OnIndexChanged))]
        private int _selectedIndex;

        [Inject]
        private void Construct(NetworkManager networkManager) => _networkManager = networkManager;

        public override void OnStartServer()
        {
            _interactiveButtonIncrease.Setup(NextScene);
            _interactiveButtonDecrease.Setup(PrevScene);
        }

        public override void OnStartClient() => UpdateDisplay();


        [Server]
        private void NextScene()
        {
            _selectedIndex++;
            if (_selectedIndex >= _sceneNames.Length) _selectedIndex = 0;
        }

        [Server]
        private void PrevScene()
        {
            _selectedIndex--;
            if (_selectedIndex < 0) _selectedIndex = _sceneNames.Length - 1;
        }

        private void OnIndexChanged(int oldIndex, int newIndex) => UpdateDisplay();

        private void UpdateDisplay()
        {
            int safeIndex = Mathf.Clamp(_selectedIndex, 0, _sceneNames.Length - 1);
            InteractionPrompt = _sceneNames[safeIndex];
        }

        private string GetCurrentSceneName() => _sceneNames[Mathf.Clamp(_selectedIndex, 0, _sceneNames.Length - 1)];

        #region IInteractable

        [field: SerializeField] public Transform InteractionAnchor { get; private set; }

        public string InteractionPrompt { get; private set; }

        public bool CanInteract => _sceneNames.Length > 0;

        public void OnInteract(GameObject interactor)
        {
            string targetScene = GetCurrentSceneName();

            _networkManager.ServerChangeScene(targetScene);
        }

        #endregion
    }
}