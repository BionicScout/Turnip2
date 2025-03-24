using UnityEngine;
using UnityEngine.UI;

public class BuilderItem : MyTabButton {
    public Image objectImage;

    public void UpdateSprite(Sprite sprite) {
        objectImage.sprite = sprite;
    }

    public Sprite GetSprite() {
        return objectImage.sprite;
    }
}
