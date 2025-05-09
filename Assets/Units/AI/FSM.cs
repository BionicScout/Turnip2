using System.Diagnostics;
using UnityEngine.UI;

public class FSM {
    public FSM_State currentState = null;

    public FSM(FSM_State startState) {
        Debug.Assert(startState != null);
        currentState = startState;
        currentState.OnEnter();
    }

    public void Update() {
        currentState.Move();
        currentState.UseItem();
        currentState.Attack();
        currentState.Transition();
    }


    public void ChangeStates(FSM_State newState) {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}
