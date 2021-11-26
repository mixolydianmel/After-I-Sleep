using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableElement : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Item Data")]
    [SerializeField] private Canvas canvas;

    private CanvasGroup canvasGroup;
    private RectTransform rt;

    private void Start() {
        rt = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void OnPointerDown(PointerEventData evData) {
        Debug.Log("OnPointerDown");
    }
    
    public void OnBeginDrag(PointerEventData evData) {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.75f;
    }
    
    public void OnEndDrag(PointerEventData evData) {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
    
    public void OnDrag(PointerEventData evData) {
        rt.anchoredPosition += evData.delta / canvas.scaleFactor;
    }
}
