using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    // Singleton
    [HideInInspector] public static GameManager Instance;

    private static List<float> values;
    private static int currentValueIndex;
    
    void Awake() {
        // Enforce Singleton
        if (Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    void Start() {
        values = new List<float>();
        currentValueIndex = int.MaxValue - 1;
    }
    
    float GetNextValue() {
        currentValueIndex++;
        if (currentValueIndex >= values.Count) {
            currentValueIndex = 0;
        }
        return values[currentValueIndex];
    }
    
    void AddValue(float v) {
        values.Add(v);
    }
    
    void RemoveValue(float v) {
        values.Remove(v);
    }
}
