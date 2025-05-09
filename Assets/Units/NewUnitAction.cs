using UnityEngine;

//Action Strategy
public interface IUnitActionStrategy {
    public string GetName();
    public Sprite GetUISprite();
    public bool Execute(UnitModel user, object input);
}

public abstract class UnitActionStrategySO : ScriptableObject, IUnitActionStrategy {
    public string Name;
    public Sprite UISprite;

    public string GetName() => Name;
    public Sprite GetUISprite() => UISprite;

    public abstract bool Execute(UnitModel user, object input);
}

//Action Factory
//public class ActionFactory : MonoBehaviour {
//    [SerializeField] List<ActionEntry> actionRegistry = new List<ActionEntry>();

//    [System.Serializable]
//    private class ActionEntry {
//        public string actionId;
//        public UnitActionStrategySO action;
//    }

//    private Dictionary<string, IUnitActionStrategy> actionCache;

//    public static ActionFactory Instance { get; private set; }

//    private void Awake() {
//        Debug.Assert(Instance == null, "ActionFactory: There are are multiple instance of ActionFactory");
//        Instance = this;

//        actionCache = new Dictionary<string, IUnitActionStrategy>();
//        foreach(var entry in actionRegistry) {
//            if(entry.action != null)
//                actionCache[entry.actionId] = entry.action;
//        }
//    }

//    public void RegisterAction(string actionId, IUnitActionStrategy action) {
//        if(!actionCache.ContainsKey(actionId)) {
//            actionCache[actionId] = action;
//            return;
//        }

//        Debug.LogError("Action " + actionId + " is already added");

//    }

//    public IUnitActionStrategy GetAction(string actionId) {
//        if(actionCache.TryGetValue(actionId, out var action)) {
//            return action;
//        }

//        Debug.LogError("Action " + actionId + " does not exist");
//        return null;
//    }
//}