using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableElement : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Item Data")]
    [SerializeField] private Canvas canvas;
    
    [SerializeField] private float value = 0;

    private CanvasGroup canvasGroup;
    private RectTransform rt;

    private void Start() {
        rt = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    // Keeping this here for the future in case we need clickables
    public void OnPointerDown(PointerEventData evData) {
        //Debug.Log("OnPointerDown");
        return;
    }
    
    // Called when the user starts dragging the object
    public void OnBeginDrag(PointerEventData evData) {
        canvasGroup.blocksRaycasts = false; // For OnDrop later in SlotElement
        canvasGroup.alpha = 0.75f;
    }
    
    // Called when the user stops dragging the object
    public void OnEndDrag(PointerEventData evData) {
        canvasGroup.blocksRaycasts = true; // Needed to pick up the object again
        canvasGroup.alpha = 1f;
    }
    
    // Called each drag frame
    public void OnDrag(PointerEventData evData) {
        rt.anchoredPosition += evData.delta / canvas.scaleFactor; // Scale by canvas size
    }
    
    // Retrieves the "value" field
    public float GetValue() {
        return value;
    }
}
