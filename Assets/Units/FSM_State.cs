public abstract class FSM_State {
    public abstract void OnEnter();
    public abstract void Move();
    public abstract void UseItem();
    public abstract void Attack();
    public abstract void Transition();
    public abstract void OnExit();
}