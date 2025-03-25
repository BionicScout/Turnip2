using UnityEngine;

public class LevelLoader : MonoBehaviour {
    public string levelToLoad;
    [SerializeField] private Sprite DefualtSprite;
    [SerializeField] private GameObject HexPrefab;

    private void Awake() {
        DataSaver saver = new DataSaver();
        LevelBuilderData data = saver.Load(levelToLoad);
        GameObject gameObject = new GameObject("HexGrid");
        HexGrid grid = gameObject.AddComponent<HexGrid>();
        grid.Instance(HexPrefab, DefualtSprite, data);
    }
}
