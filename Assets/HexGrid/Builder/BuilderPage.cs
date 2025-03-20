using System.Collections.Generic;
using UnityEngine;

public class BuilderPage : MonoBehaviour {
    public List<BuilderItem> builderItems = new List<BuilderItem>();

    public List<Sprite> sprites = new List<Sprite>();

    private void Start() {
        for(int i = 0; i < Mathf.Min(builderItems.Count, sprites.Count); i++) {
            builderItems[i].UpdateSprite(sprites[i]);
        }
    }
}
