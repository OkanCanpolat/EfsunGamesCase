using System;
using UnityEngine;
using Zenject;

public abstract class FactoryBase : MonoBehaviour, IClickable, IDataPersistence
{
    public Action<int> OnStorageChange;
    public Action<int> OnTimeChange;
    public Action OnGenerationStart;
    public Action OnGenerationEnd;
    public Action OnGenerate;

    [SerializeField] protected string id;
    [SerializeField] protected FactoryConfig config;
    [Inject] protected Inventory inventory;
    [Inject] protected GameTimeController timeController;
    [Inject] protected DataPersistenceManager persistenceManager;
    [Inject] protected FactoryClickedSignal clickedSignal;
    [Inject] protected SignalBus signalBus;
    [Inject] protected IAudioService audioService;
    [Inject] protected SoundConfig soundConfig;
    protected int currentResourceAmount;
    protected int elapsedTime;
    public int CurrentResourceAmount => currentResourceAmount;
    public int ElapsedTime => elapsedTime;

    protected virtual void Awake()
    {
        persistenceManager.RegisterDataPersistence(this);
    }

    [ContextMenu ("Generate Guid")]
    private void GenerateGuid()
    {
        id = Guid.NewGuid().ToString();
    }
    public FactoryConfig Config => config;
    public abstract void GenerateResource();
    public virtual void OnClick()
    {
        clickedSignal.ClickedFactory = this;
        signalBus.TryFire(clickedSignal);
    }
    public abstract void SavaData(GameData gameData);
    public abstract void LoadData(GameData gameData);
}
