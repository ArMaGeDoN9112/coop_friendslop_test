using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Coop.UI
{
    namespace Coop.UI
    {
        public class CustomNetworkHUD : MonoBehaviour
        {
            [Header("UI References")] [SerializeField]
            private TMP_InputField _inputFieldAddress;

            [SerializeField] private Button _buttonHost;
            [SerializeField] private Button _buttonClient;
            [SerializeField] private Button _buttonServerOnly;
            [SerializeField] private GameObject _panelConnection;
            [SerializeField] private Button _buttonStop;

            private NetworkManager _networkManager;

            [Inject]
            public void Construct(NetworkManager networkManager) => _networkManager = networkManager;

            private void Start()
            {
                Debug.Log(_networkManager);

                if (_inputFieldAddress)
                {
                    _inputFieldAddress.text = "localhost";
                    _inputFieldAddress.onValueChanged.AddListener(OnAddressChanged);
                }

                _buttonHost.onClick.AddListener(OnClickHost);
                _buttonClient.onClick.AddListener(OnClickClient);
                _buttonServerOnly.onClick.AddListener(OnClickServer);
                _buttonStop.onClick.AddListener(OnClickStop);

                UpdateUIState();
            }

            private void Update() => UpdateUIState();

            private void UpdateUIState()
            {
                if (!NetworkClient.isConnected && !NetworkServer.active)
                {
                    _panelConnection.SetActive(true);
                    _buttonStop.gameObject.SetActive(false);
                }
                else
                {
                    _panelConnection.SetActive(false);
                    _buttonStop.gameObject.SetActive(true);
                }
            }

            private void OnAddressChanged(string address) => _networkManager.networkAddress = address;

            private void OnClickHost() => _networkManager.StartHost();

            private void OnClickClient() => _networkManager.StartClient();

            private void OnClickServer() => _networkManager.StartServer();

            private void OnClickStop()
            {
                if (NetworkServer.active && NetworkClient.isConnected)
                    _networkManager.StopHost();
                else if (NetworkClient.isConnected)
                    _networkManager.StopClient();
                else if (NetworkServer.active) _networkManager.StopServer();
            }
        }
    }
}