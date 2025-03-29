using UnityEngine;

public class LevelLoader : MonoBehaviour {
    public string levelToLoad;
    [SerializeField] private Sprite DefualtSprite;
    [SerializeField] private GameObject HexPrefab;

    HexGrid grid;

    private void Awake() {
        DataSaver saver = new DataSaver();
        LevelBuilderData data = saver.Load(levelToLoad);
        GameObject gameObject = new GameObject("HexGrid");
        grid = gameObject.AddComponent<HexGrid>();
        grid.Instance(HexPrefab, DefualtSprite, data);
    }

    private void Start() {
        GameEvents.current.SetCameraBounds(grid);
    }
}
