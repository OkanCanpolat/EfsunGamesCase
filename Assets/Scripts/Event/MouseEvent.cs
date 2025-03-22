using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class MouseEvent : MonoBehaviour
{
    [SerializeField] private LayerMask clickableLayer;
    [Inject] private SignalBus signalBus;
    private Camera mainCam;
    private EventSystem eventSystem;
    private void Awake()
    {
        mainCam = Camera.main;
        eventSystem = EventSystem.current;
    }
    private void Update()
    {
        ControlClick();
    }

    private void ControlClick()
    {
        if (IsPointerOverUI()) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool cast = Physics.Raycast(ray, out hit, 1000f, clickableLayer);

            if (cast)
            {
                IClickable clickable = hit.transform.GetComponent<IClickable>();

                if(clickable != null)
                {
                    clickable.OnClick();
                }
            }

            else
            {
                signalBus.TryFire<EmptyClickSignal>();
            }
        }
    }

    private bool IsPointerOverUI()
    {
        return eventSystem.IsPointerOverGameObject();
    }
}
