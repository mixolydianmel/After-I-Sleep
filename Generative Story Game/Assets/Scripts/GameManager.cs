using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    // Singleton
    [HideInInspector] public static GameManager Instance;
    [HideInInspector] public static int day;

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
    
    public float GetNextValue() {
        currentValueIndex++;
        if (currentValueIndex >= values.Count) {
            currentValueIndex = 0;
        }
        return values[currentValueIndex];
    }
    
    public void AddValue(float v) {
        values.Add(v);
    }
    
    public void RemoveValue(float v) {
        values.Remove(v);
    }

    public void TitleScreen()
    {
        values = new List<float>();
        SceneManager.LoadScene("Title Screen");
    }

    public void CreationScreen() {
        values = new List<float>();
        SceneManager.LoadScene("Creation");
    }

    public void GenerateWorld() {
        GameObject.Find("Slots").GetComponent<SlotsManager>().ReportValues();
        SceneManager.LoadScene("Prototype");
    }
}
