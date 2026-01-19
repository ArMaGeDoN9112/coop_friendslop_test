using Coop.Configs;
using Coop.Interaction;
using Coop.Interaction.UI;
using Coop.Scene;
using UnityEngine;
using Zenject;

namespace Coop.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private SceneScript sceneScript;
        [SerializeField] private InteractionHintView _hintPrefab;

        public override void InstallBindings()
        {
            Container.Bind<SceneScript>().FromInstance(sceneScript).AsSingle();
            Container.BindInterfacesTo<MirrorZenjectBridge>().AsSingle().NonLazy();

            Container.Bind<InteractionHintView>().FromComponentInNewPrefab(_hintPrefab).AsSingle();
            Container.BindInterfacesTo<InteractionUIService>().AsSingle();


            Container.Bind<Camera>().FromComponentInHierarchy().AsCached();
        }
    }
}