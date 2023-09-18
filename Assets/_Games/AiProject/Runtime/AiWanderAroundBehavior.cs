using UnityEngine;

public class AiWanderAroundBehavior : IAiBehavior
{
    private float _timeToWait;

    public void OnStart(AiAgent agent)
    {
        var targetPosition = new Vector3(
                 Random.Range(-24f, 24f),
                 0f,
                 Random.Range(-24f, 24f)
                 );

        agent.SetDestination(targetPosition);
    }

    public bool Execute(AiAgent agent)
    {
        if (_timeToWait > 0f)
        {
            _timeToWait -= Time.deltaTime;
            return _timeToWait <= 0f;
        }

        if (!agent.IsAtDestination())
        {
            return false;
        }

        _timeToWait = Random.Range(2f, 5f);
        return false;
    }
}