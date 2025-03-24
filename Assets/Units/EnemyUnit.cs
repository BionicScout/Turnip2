using Unity.VisualScripting;

public class EnemyUnit : CombatUnit {
    FSM fsm;

    private void Start() {
        fsm = new FSM(null);
    }

    public void ExecuteTurn() {
        fsm.Update();
    }


    /************************************
        FSM STATES
    ************************************/

    public class MeleeAttackState : FSM_State {

        public override void OnEnter() {
            throw new System.NotImplementedException();
        }

        public override void Move() {
            //If not next to enemy
                //Find cloest player
                //Find Move best move spot around player
                //If enemy can move next to player this turn, priortize those move spots.
                //Pathfind to player
                //Move Towards player
            //If next to enemy
                //Try to move to best attack spot
            throw new System.NotImplementedException();
        }
        public override void UseItem() {
            //Use healing item if hurt
            throw new System.NotImplementedException();
        }

        public override void Attack() {
            //Get list of adjuacent player's
            //Select player with lowest health (or different condtion maybe)
            //Attack selected player (if any)
            throw new System.NotImplementedException();
        }   

        public override void Transition() {
            throw new System.NotImplementedException();
        }

        public override void OnExit() {
            throw new System.NotImplementedException();
        }

    }

    public class RangeAttackState : FSM_State {
        public override void OnEnter() {
            throw new System.NotImplementedException();
        }

        public override void Move() {
            //If not next to enemy
            //Find cloest player
            //Find Move best move spot in range, but furthest away as possible
            //Pathfind to best spot
            //Move Towards spot
            throw new System.NotImplementedException();
        }
        public override void UseItem() {
            //Use healing item if hurt
            throw new System.NotImplementedException();
        }

        public override void Attack() {
            //Get list of all player's in range player's
            //Select player with lowest health (or different condtion maybe)
            //Attack selected player (if any)
            throw new System.NotImplementedException();
        }

        public override void Transition() {
            throw new System.NotImplementedException();
        }

        public override void OnExit() {
            throw new System.NotImplementedException();
        }
    }

    public class RetreatState : FSM_State {
        public override void OnEnter() {
            throw new System.NotImplementedException();
        }

        public override void Move() {
            throw new System.NotImplementedException();
        }
        public override void UseItem() {
            throw new System.NotImplementedException();
        }

        public override void Attack() {
            throw new System.NotImplementedException();
        }

        public override void Transition() {
            throw new System.NotImplementedException();
        }

        public override void OnExit() {
            throw new System.NotImplementedException();
        }
    }

}