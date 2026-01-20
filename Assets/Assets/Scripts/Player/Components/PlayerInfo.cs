using Mirror;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Coop.Player.Components
{
    public class PlayerInfo : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnNameChanged))]
        public string PlayerName;

        [Header("Visuals")] [SerializeField] private Renderer _bodyRenderer;
        [SerializeField] private TMP_Text _nameLabel3D;

        private void Update()
        {
            if (isLocalPlayer) return;

            _nameLabel3D.transform.LookAt(Camera.main.transform);
        }

        public override void OnStartLocalPlayer()
        {
            string genName = "Player" + Random.Range(100, 999);
            CmdSetupPlayer(genName);
        }

        [Command]
        private void CmdSetupPlayer(string name) => PlayerName = name;

        private void OnNameChanged(string _, string newName)
        {
            _nameLabel3D.text = newName;
            gameObject.name = newName;
        }
    }
}