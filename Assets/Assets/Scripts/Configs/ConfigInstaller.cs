using UnityEngine;
using Zenject;

namespace Coop.Configs
{
    [CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Configs/ConfigInstaller")]
    public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
    {
        [SerializeField] private PlayerConfig _playerConfig;
        public override void InstallBindings() { Container.BindInstance(_playerConfig).AsSingle(); }
    }
}