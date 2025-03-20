using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour {


    public int minimum;
    public int maximum;
    public int current;
    public Image mask;
    public Image fill;
    public Color color;

    private void Update() {
        GetCurrentFill();
    }

    private void GetCurrentFill() {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset/maximumOffset;
        mask.fillAmount = fillAmount;

        fill.color = color; 
    }

    public void SetCurrentFill(int value) {
        current = value;
    }
}
