using Coop.Configs;
using UnityEngine;
using Zenject;

namespace Coop.Installers
{
    [CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Configs/ConfigInstaller")]
    public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
    {
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private BombConfig _bombConfig;
        [SerializeField] private PrefabsConfig _prefabsConfig;
        [SerializeField] private BombDispencerConfig _bombDispencerConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_playerConfig).AsSingle();
            Container.BindInstance(_bombConfig).AsSingle();
            Container.BindInstance(_prefabsConfig).AsSingle();
            Container.BindInstance(_bombDispencerConfig).AsSingle();
        }
    }
}