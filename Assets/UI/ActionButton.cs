using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour {
    public TMP_Text text;
    public int index;

    public event Action<int> OnButtonPressed = delegate { };

    void Start() {
        GetComponent<Button>().onClick.AddListener(() => OnButtonPressed(index));
    }

    public void RegisterListener(Action<int> listener) {
        OnButtonPressed += listener;
    }

    public void Initialize(int index, string text) {
        this.index = index;
        this.text.text = text;
    }
}
