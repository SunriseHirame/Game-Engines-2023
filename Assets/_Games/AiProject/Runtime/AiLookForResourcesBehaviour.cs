using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiLookForResourcesBehaviour : IAiBehavior
{
    private Interatable _targetInteractable;
    private bool _isInteracting;

    private string _statName;

    private float _basePriority;
    private float _priotyMultiplier;

    public AiLookForResourcesBehaviour (string statName, float basePriority, float priorityMultiplier)
    {
        _statName = statName;
        _basePriority = basePriority;
        _priotyMultiplier = priorityMultiplier;
    }

    public float Evaluate(AiAgent agent)
    {
        if (_isInteracting) return 1000f;

        if (!Interatable.IsThereAnyWithType(_statName)) return float.MinValue;

        if (agent.TryGetStat(_statName, out var value))
        {
            return (_basePriority - value) * _priotyMultiplier;
        }
        return 0f;
    }

    public void OnStart(AiAgent agent)
    {
        _isInteracting = false;

        var allEnergyOrbs = Interatable.All;

        var distanceToClosest = float.MaxValue;
        var closestObject = default(Interatable);

        foreach (var energyOrb in allEnergyOrbs)
        {
            if (!energyOrb.GivesStats) continue;
            if (energyOrb.StatName != _statName) continue;

            var distance = Vector3.Distance(agent.transform.position, energyOrb.transform.position);
            if (distance < distanceToClosest)
            {
                closestObject = energyOrb;
                distanceToClosest = distance;
            }
        }

        _targetInteractable = closestObject;
        if (_targetInteractable)
        {
            agent.SetDestination(_targetInteractable.InteractionPoint);
            agent.SetStoppingDistance(_targetInteractable.InteractableRadius);
        }
    }


    public bool Execute(AiAgent agent)
    {
        if (!_targetInteractable) return true;        

        if (agent.IsAtDestination())
        {
            if (_isInteracting)
            {
                if (!agent.IsInteracting)
                {
                    if (_targetInteractable.GivesStats)
                    {
                        agent.AddStat(_targetInteractable.StatName, _targetInteractable.StatAmount);
                    }

                    agent.StartCoroutine(_targetInteractable.ReactiveAfterDelay(Random.Range(5f, 10f)));

                    _targetInteractable.gameObject.SetActive(false);
                    _targetInteractable = null;


                    return true;
                }
            }
            else if (_targetInteractable.TryGetComponent<Interatable> (out var interactable))
            {
                agent.PlayAnimation(interactable.InteractionAnimationName);
                _isInteracting = true;
            }
        }

        return agent.Energy > 50f;
    }
    public void OnExit(AiAgent agent)
    {
        _isInteracting = false;
        _targetInteractable = null;
    }


    public override string ToString()
    {
        return $"{base.ToString()}::{_statName}";
    }
}
