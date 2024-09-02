using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UnityEvent onHold;
    [SerializeField] private UnityEvent onRelease;

    private bool isHolding = false;

    private void Update()
    {
        if (isHolding)
        {
            onHold?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        onRelease?.Invoke();
    }
}
