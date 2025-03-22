using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private ResourcesUIConfig resourcesUIConfig;
    [SerializeField] private SoundConfig soundConfig;
    [Header ("Save System")]
    [SerializeField] private string saveFileName;
    [SerializeField] private bool useEncryption;
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<FactoryClickedSignal>().OptionalSubscriber();
        Container.DeclareSignal<EmptyClickSignal>().OptionalSubscriber();
        Container.Bind<FactoryClickedSignal>().AsSingle();

        Container.Bind<SoundConfig>().FromInstance(soundConfig).AsSingle();
        Container.Bind<IAudioService>().To<AudioManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ResourcesUIConfig>().FromInstance(resourcesUIConfig).AsSingle();
        Container.Bind<GameTimeController>().AsSingle();
        Container.Bind<IDataHandler>().To<FileDataHandler>().AsSingle().WithArguments(Application.persistentDataPath, saveFileName, useEncryption);
        Container.Bind<Inventory>().AsSingle();
        Container.Bind<DataPersistenceManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ResourceIndicator>().FromComponentsInHierarchy().AsSingle();
    }
}
public class FactoryClickedSignal
{
    public FactoryBase ClickedFactory;
}
public class EmptyClickSignal { }
