using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionsOrganizer : MonoBehaviour
{
    [Header ("Organizer")]
    public RectTransform rectTransform;
    
    void Start() {
        // == Organize Tiles ==
        Vector2 placement = Vector2.zero; 
        
        // Place children
        foreach (RectTransform child in GetComponentInChildren<RectTransform>()) {
            placement = new Vector2(rectTransform.localPosition.x + Random.Range(0f, rectTransform.rect.width),
                                    rectTransform.localPosition.y - Random.Range(0f, rectTransform.rect.height));
            child.localPosition = placement;
        }
    }
}
