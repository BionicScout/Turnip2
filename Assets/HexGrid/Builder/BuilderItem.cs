using UnityEngine;
using UnityEngine.UI;

public class BuilderItem : MonoBehaviour {
    public Image image;

    public void UpdateSprite(Sprite sprite) {
        image.sprite = sprite;
    }
}
