public class ReinforcementProblem : StateSystem
{
    public virtual GameState GetRandomState()
    {
        // TODO
        // Define behaviour
        return new GameState();
    }
    public virtual GameAction[] GetAvailableActions(GameState s)
    {
        //TODO
        //Define behaviour
        return new GameAction[0];
    }

    public virtual GameState TakeAction(GameState s, GameAction a, ref float reward)
    {
        //TODO Def behaviour
        reward = 0f;
        return new GameState();
    }
}