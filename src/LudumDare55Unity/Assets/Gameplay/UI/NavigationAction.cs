using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavigationAction : Selectable
{
    [SerializeField] private UnityEvent OnNavigate;

    private EventSystem eventSystem;
    private GameObject lastSelectedGameObject;

    protected override void Start()
    {
        base.Start();
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (eventSystem == null)
        {
            return;
        }
        if (eventSystem.currentSelectedGameObject != lastSelectedGameObject)
        {
            if (eventSystem.currentSelectedGameObject == gameObject)
            {
                eventSystem.SetSelectedGameObject(lastSelectedGameObject);
                OnNavigate?.Invoke();
            }
            lastSelectedGameObject = eventSystem.currentSelectedGameObject;
        }
    }
}
