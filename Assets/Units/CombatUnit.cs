using UnityEngine;

public abstract class CombatUnit : HexObject {
    public enum State { move = 0, item = 1, action = 2, done = 3 };

    public State state;
    public Sprite unitSprite;
    public int startMovement;
    public int currentMovement;
}
