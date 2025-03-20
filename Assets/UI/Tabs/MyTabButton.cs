using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MyTabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler {
    public TabGroup tabGroup;

    public Image background;

    public UnityEvent OnTabSelected;
    public UnityEvent OnTabDeselected;

    public void OnPointerEnter(PointerEventData eventData) {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerClick(PointerEventData eventData) {
        tabGroup.OnTabSelect(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        tabGroup.OnTabExit(this);
    }

    private void Start() {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }

    public void Select() {
        if(OnTabSelected != null) {
            OnTabSelected.Invoke();
        }
    }

    public void Deselect() {
        if(OnTabDeselected != null) {
            OnTabDeselected.Invoke();
        }
    }
}
