using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AdjustableFactoryUIController : BasicFactoryUIController
{
    [Header("Production Settings")]
    [SerializeField] private Button queueAddButton;
    [SerializeField] private Button queueRemoveButton;
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private TMP_Text capacityText;
    [SerializeField] private TMP_Text targetAmountText;
    [SerializeField] private TMP_Text outputAmountText;
    [SerializeField] private Image addButtonOutputImage;
    [Inject] private Inventory inventory;
    [Inject] private SignalBus signalBus;
    [Inject] private IAudioService audioService;
    [Inject] private SoundConfig soundConfig;
    private AdjustableFactory adjustableFactory;
    protected override void Awake()
    {
        base.Awake();

        signalBus.Subscribe<EmptyClickSignal>(CloseButtons);
        signalBus.Subscribe<FactoryClickedSignal>(OnMouseClick);
        adjustableFactory = factory as AdjustableFactory;

        adjustableFactory.OnAddQueue += OnAddQueue;
        adjustableFactory.OnRemoveQueue += OnRemoveQueue;
        adjustableFactory.Clicked += OpenProductionPanel;
        adjustableFactory.OnCollect += OnCollect;

        queueAddButton.onClick.AddListener(adjustableFactory.AddQueue);
        queueAddButton.onClick.AddListener(() => audioService.PlaySoundOnce(soundConfig.ButtonClickSound, soundConfig.ButtonClickVolume));
        queueRemoveButton.onClick.AddListener(adjustableFactory.RemoveQueue);
        queueRemoveButton.onClick.AddListener(() => audioService.PlaySoundOnce(soundConfig.ButtonClickSound, soundConfig.ButtonClickVolume));
    }
    private void ControlButtons()
    {
        ControlRemoveButton();
        ControlAddButton();
    }
    private void ControlAddButton()
    {
        int inventoryCount = inventory.GetResourceAmount(factory.Config.Requirement.Type);

        if (inventoryCount >= factory.Config.Requirement.Amount && adjustableFactory.GenerationQueueCount + adjustableFactory.CurrentResourceAmount < adjustableFactory.Config.Capacity)
        {
            queueAddButton.interactable = true;
        }
        else
        {
            queueAddButton.interactable = false;
        }
    }
    private void ControlRemoveButton()
    {
        if (adjustableFactory.GenerationQueueCount > 1)
        {
            queueRemoveButton.interactable = true;
        }
        else
        {
            queueRemoveButton.interactable = false;
        }
    }
    public void OnAddQueue()
    {
        targetAmountText.text = (adjustableFactory.GenerationQueueCount + factory.CurrentResourceAmount).ToString();
       
        ControlRemoveButton();
        ControlAddButton();
    }
    public override void InitUI()
    {
        capacityText.text = factory.Config.Capacity.ToString();
        targetAmountText.text = "0";
        storageText.text = factory.CurrentResourceAmount.ToString();
        outputAmountText.text = factory.Config.Requirement.Amount.ToString();
        addButtonOutputImage.sprite = factory.Config.Requirement.Sprite;
        timeText.text = "";
        timeSlider.value = 1f;

        if (adjustableFactory.GenerationQueueCount > 0)
        {
            targetAmountText.text = (adjustableFactory.GenerationQueueCount + adjustableFactory.CurrentResourceAmount).ToString();
        }

        if (factory.CurrentResourceAmount >= factory.Config.Capacity)
        {
            timeText.text = "FULL";
        }
    }
    public void OnRemoveQueue()
    {
        targetAmountText.text = (adjustableFactory.GenerationQueueCount + factory.CurrentResourceAmount).ToString();
       
        ControlRemoveButton();
        ControlAddButton();
    }
    public override void OnGenerationEnd()
    {
        timeSlider.value = 1f;

        targetAmountText.text = "0";

        if (factory.CurrentResourceAmount >= factory.Config.Capacity)
        {
            timeText.text = "FULL";
        }
        else
        {
            timeText.text = "";
        }
    }
    private void OnCollect(int colelctedAmount)
    {
        targetAmountText.text = (adjustableFactory.GenerationQueueCount + factory.CurrentResourceAmount).ToString();
       
        if(adjustableFactory.GenerationQueueCount <= 0)
        {
            timeText.text = "";
        }
       
        ControlAddButton();
    }
    private void OnMouseClick(FactoryClickedSignal signal)
    {
        if (signal.ClickedFactory != factory && buttonsParent.activeSelf)
        {
            CloseButtons();
        }
    }
    private void CloseButtons()
    {
        if (!buttonsParent.activeSelf) return;

        buttonsParent.SetActive(false);
        adjustableFactory.CanCollect = false;
    }
    private void OpenProductionPanel()
    {
        if (adjustableFactory.CanCollect) return;
        buttonsParent.SetActive(true);
        adjustableFactory.CanCollect = true;
        ControlButtons();
    }
}
