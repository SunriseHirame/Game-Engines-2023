using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public interface IAiBehavior
{
    void OnStart(AiAgent agent);
    bool Execute(AiAgent agent);
}

public class AiCommandedBehavior : IAiBehavior
{
    public void OnStart(AiAgent agent)
    {
    }

    public bool Execute (AiAgent agent)
    {
        if (agent.IsAtDestination())
        {
            return false;
        }

        return true;
    }
}
