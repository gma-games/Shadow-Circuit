using System;
using System.Collections.Generic;

public enum EnemyState
{
    Patrol, // Őrjáratozás
    Chase,  // Játékos üldözése
    Search  // Játékos keresése, miután elvesztette
}

public class FiniteStateMachine
{
    public EnemyState currentState;

    // Funkció összekötése az állapoptokkal, bővíthető
    public Dictionary<EnemyState, Action> stateActions = new Dictionary<EnemyState, Action>();

    public void ChangeState(EnemyState newState)
    {
        currentState = newState;
        // AKár animáció váltás
    }

    public void ExecuteState()
    {
       // Ha van az állapothoz funckó akkor annak a lefuttatása
        if (stateActions.ContainsKey(currentState))
        {
            stateActions[currentState]?.Invoke();
        }
    }
}