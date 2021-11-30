using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotElement : MonoBehaviour, IDropHandler
{
    private RectTransform rt;

    void Start() {
        rt = GetComponent<RectTransform>();
    }

    // Called when a draggable element is dropped on the slot
    public void OnDrop(PointerEventData evData) { 
        // If the pointer is dragging something when it is dropped
        if (evData.pointerDrag != null) {
            evData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = rt.anchoredPosition;
        }
    }
}
