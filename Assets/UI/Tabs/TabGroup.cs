using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour {
    public List<MyTabButton> tabButtons;
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;

    public List<GameObject> objectsToSwap;

    protected MyTabButton selectedTab;

    public void Subscribe(MyTabButton button) {
        if(tabButtons == null) {
            tabButtons = new List<MyTabButton>();
        }

        tabButtons.Add(button);
        button.background.color = tabIdle;
    }

    public void OnTabEnter(MyTabButton button) {
        ResetTabs();
        if(selectedTab == null || button != selectedTab) {
            button.background.color = tabHover;
        }
    }

    public void OnTabSelect(MyTabButton button) {
        if(selectedTab != null) {
            selectedTab.Deselect();
        }
        selectedTab = button;
        selectedTab.Select();
        ResetTabs();
        button.background.color = tabActive;

        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++) {
            objectsToSwap[i].SetActive(index == i);
        }
    }

    public void OnTabExit(MyTabButton button) {
        ResetTabs();
    }

    public void ResetTabs() {
        foreach(MyTabButton button in tabButtons) {

            if(selectedTab != null && selectedTab == button) {
                continue;
            }

            button.background.color = tabIdle;
        }
    }
}
