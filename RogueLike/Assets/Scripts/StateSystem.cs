using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSystem : MonoBehaviour
{
    public struct GameState
    {
        // TODO
        // state definition here 
    }

    public struct GameAction
    {
        // TODO
        // action definition here
    }

    public class ReinforcementProblem
    {

    }

    public virtual GameState GetRandomState()
    {
        //TODO
        //Define behaviour

        return new GameState();
    }

    public virtual GameAction[] GetAvaliableActions(GameState s)
    {
        //TODO
        //Define behaviour
        return new GameAction[0];
    }

    public virtual GameState TakeAction(GameState s, GameAction a, ref float reward)
    {
        reward = 0f;
        return new GameState();
    }
}