using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    private SlotElement[] slots;

    void Start()
    {
        slots = GetComponentsInChildren<SlotElement>();
    }

    public void ReportValues() {
        foreach (SlotElement s in slots) {
            GameManager.Instance.AddValue(s.GetValue());
        }
    }
}
