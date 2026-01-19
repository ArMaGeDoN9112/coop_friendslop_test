using Coop.Interaction;
using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Scene.Lobby
{
    public class NetworkLobbyButton : MonoBehaviour, IInteractable
    {
        [Header("Settings")] [SerializeField] private NetworkActionType _actionType;
        [SerializeField] private string _promptText = "Press E";
        [SerializeField] private string _ipAddressToSet = "localhost";

        [Header("Visuals")] [SerializeField] private Transform _anchor;

        private NetworkManager _networkManager;


        [Inject]
        public void Construct(NetworkManager networkManager) { _networkManager = networkManager; }


        #region IInteractable

        public string InteractionPrompt => _promptText;

        public bool CanInteract => true;

        public Transform InteractionAnchor => _anchor;

        public void OnInteract(GameObject interactor) => ExecuteAction();

        #endregion

        private void ExecuteAction()
        {
            switch (_actionType)
            {
                case NetworkActionType.Host:
                    _networkManager.StartHost();
                    break;
                case NetworkActionType.Client:
                    _networkManager.StartClient();
                    break;
                case NetworkActionType.ServerOnly:
                    _networkManager.StartServer();
                    break;
                case NetworkActionType.Stop:
                    if (NetworkServer.active && NetworkClient.isConnected)
                        _networkManager.StopHost();
                    else if (NetworkClient.isConnected)
                        _networkManager.StopClient();
                    else if (NetworkServer.active) _networkManager.StopServer();
                    break;
                case NetworkActionType.SetAddress:
                    _networkManager.networkAddress = _ipAddressToSet;
                    break;
            }
        }
    }
}