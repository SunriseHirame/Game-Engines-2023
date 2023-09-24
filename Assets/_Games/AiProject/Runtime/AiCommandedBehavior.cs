using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public interface IAiBehavior
{
    float Evaluate(AiAgent agent);

    void OnStart(AiAgent agent);
    bool Execute(AiAgent agent);
    void OnExit(AiAgent agent);
}

public class AiCommandedBehavior : IAiBehavior
{
    private bool _isActive;

    public float Evaluate(AiAgent agent)
    {
        return _isActive ? float.MaxValue : float.MinValue;
    }

    public void OnStart(AiAgent agent)
    {
        _isActive = true;
    }

    public void OnExit (AiAgent agent)
    {
        _isActive = false;
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
