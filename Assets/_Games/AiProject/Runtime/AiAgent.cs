using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_navAgent;

    private AiCommandedBehavior _commandedBehvior = new AiCommandedBehavior();
    private AiWanderAroundBehavior _wanderAroundBehavior = new AiWanderAroundBehavior();

    private IAiBehavior _currentBehaviour;

    private void Awake()
    {
        SwitchToBehavior(_wanderAroundBehavior);
    }

    private void Update()
    {
        if (_currentBehaviour != null)
        {
            if (_currentBehaviour.Execute(this))
            {
                SwitchToBehavior(_wanderAroundBehavior);
            }
        }
    }
    public void SetDestination(Vector3 targetPosition)
    {
        m_navAgent.destination = targetPosition;
    }

    public bool IsAtDestination ()
    {
        return Vector3.Distance(m_navAgent.destination, transform.position) <= m_navAgent.stoppingDistance + 0.01f;
    }

    public void CommandMoveTo (Vector3 targetPosition)
    {
        m_navAgent.destination = targetPosition;
        SwitchToBehavior(_commandedBehvior);
    }

    private void SwitchToBehavior (IAiBehavior newBehavior)
    {
        Debug.Log($"AiAgent swithed behavior: {_currentBehaviour} -> {newBehavior}");
        _currentBehaviour = newBehavior;
        _currentBehaviour.OnStart(this);
    }
}
