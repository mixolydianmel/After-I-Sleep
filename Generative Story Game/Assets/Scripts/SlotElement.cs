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

    public void OnDrop(PointerEventData evData) { 

        if (evData.pointerDrag != null) {
            evData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = rt.anchoredPosition;
        }
    }
}
