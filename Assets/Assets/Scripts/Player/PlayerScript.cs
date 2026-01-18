// using Coop.Combat;
// using Coop.Scene;
// using Mirror;
// using TMPro;
// using UnityEngine;
//
// namespace Coop.Player
// {
//     public class PlayerScript : NetworkBehaviour
//     {
//         public Transform _headTransform;
//         public Renderer _bodyRenderer;
//         public TMP_Text playerNameText;
//         public GameObject floatingInfo;
//
//         public GameObject[] weaponArray;
//
//         [SyncVar(hook = nameof(OnNameChanged))]
//         public string playerName;
//
//         [SyncVar(hook = nameof(OnColorChanged))]
//         public Color playerColor = Color.white;
//
//         [SyncVar(hook = nameof(OnWeaponChanged))]
//         public int activeWeaponSynced;
//
//         private SceneScript _sceneScript;
//         private int _selectedWeaponLocal;
//         private Material _playerMaterialClone;
//
//         private Weapon _activeWeapon;
//         private float _weaponCooldownTime;
//         private const float MoveSpeed = 4f;
//         private const float TurnSpeed = 110f;
//
//
//         private void Awake()
//         {
//             _sceneScript = GameObject.Find("SceneReference").GetComponent<SceneReference>().sceneScript;
//
//             foreach (var item in weaponArray)
//                 if (item)
//                     item.SetActive(false);
//
//             if (_selectedWeaponLocal < weaponArray.Length && weaponArray[_selectedWeaponLocal])
//             {
//                 _activeWeapon = weaponArray[_selectedWeaponLocal].GetComponent<Weapon>();
//                 _sceneScript.UIAmmo(_activeWeapon.weaponAmmo);
//             }
//         }
//
//         private void Update()
//         {
//             if (!isLocalPlayer)
//             {
//                 // make non-local players run this
//                 floatingInfo.transform.LookAt(Camera.main.transform);
//                 return;
//             }
//
//             float z = Input.GetAxis("Vertical");
//             float x = Input.GetAxis("Horizontal");
//
//             transform.Rotate(0, x * TurnSpeed * Time.deltaTime, 0);
//             transform.Translate(0, 0, z * MoveSpeed * Time.deltaTime);
//
//             if (Input.GetButtonDown("Fire2")) //Fire2 is mouse 2nd click and left alt
//             {
//                 _selectedWeaponLocal += 1;
//
//                 if (_selectedWeaponLocal >= weaponArray.Length) _selectedWeaponLocal = 0;
//
//                 CmdChangeActiveWeapon(_selectedWeaponLocal);
//             }
//
//             if (Input.GetButtonDown("Fire1"))
//                 if (_activeWeapon && Time.time > _weaponCooldownTime && _activeWeapon.weaponAmmo > 0)
//                 {
//                     _weaponCooldownTime = Time.time + _activeWeapon.weaponCooldown;
//                     _activeWeapon.weaponAmmo -= 1;
//                     _sceneScript.UIAmmo(_activeWeapon.weaponAmmo);
//
//                     CmdShootRay();
//                 }
//         }
//
//         public override void OnStartLocalPlayer()
//         {
//             _sceneScript.playerScript = this;
//
//             Camera.main.transform.SetParent(_headTransform);
//             Camera.main.transform.localPosition = new Vector3(0, 0, 0);
//
//             floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
//             floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
//
//             string name = "Player" + Random.Range(100, 999);
//             Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
//             CmdSetupPlayer(name, color);
//         }
//
//         [Command]
//         public void CmdSendPlayerMessage()
//         {
//             if (_sceneScript) _sceneScript.statusText = $"{playerName} says hello {Random.Range(10, 99)}";
//         }
//
//         [Command]
//         private void CmdSetupPlayer(string _name, Color _col)
//         {
//             //player info sent to server, then server updates sync vars which handles it on all clients
//             playerName = _name;
//             playerColor = _col;
//             _sceneScript.statusText = $"{playerName} joined.";
//         }
//
//
//         [Command]
//         private void CmdChangeActiveWeapon(int newIndex) => activeWeaponSynced = newIndex;
//
//         [Command]
//         private void CmdShootRay()
//         {
//             var bullet = Instantiate(_activeWeapon.weaponBullet, _activeWeapon.weaponFirePosition.position,
//                 _activeWeapon.weaponFirePosition.rotation);
//
//             var rb = bullet.GetComponent<Rigidbody>();
//             rb.AddForce(bullet.transform.up * _activeWeapon.weaponSpeed, ForceMode.Impulse);
//
//             NetworkServer.Spawn(bullet);
//
//             Destroy(bullet, _activeWeapon.weaponLife);
//         }
//
//
//         private void OnNameChanged(string _Old, string _New) { playerNameText.text = playerName; }
//
//         private void OnColorChanged(Color _Old, Color _New)
//         {
//             playerNameText.color = _New;
//             _playerMaterialClone = new Material(_bodyRenderer.material) { color = _New };
//             _bodyRenderer.material = _playerMaterialClone;
//         }
//
//         private void OnWeaponChanged(int _Old, int _New)
//         {
//             // disable old weapon
//             // in range and not null
//             if (0 <= _Old && _Old < weaponArray.Length && weaponArray[_Old] != null) weaponArray[_Old].SetActive(false);
//
//             // enable new weapon
//             // in range and not null
//             if (0 <= _New && _New < weaponArray.Length && weaponArray[_New] != null)
//             {
//                 weaponArray[_New].SetActive(true);
//                 _activeWeapon = weaponArray[activeWeaponSynced].GetComponent<Weapon>();
//                 if (isLocalPlayer) _sceneScript.UIAmmo(_activeWeapon.weaponAmmo);
//             }
//         }
//     }
// }

