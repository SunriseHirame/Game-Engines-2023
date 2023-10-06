using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[System.Serializable]
public class AgentStats : System.ICloneable
{
    [field: SerializeField][field: FormerlySerializedAs("Energy")] public float Energy { get; set; }
    [field: SerializeField][field: FormerlySerializedAs("Food")] public float Food { get; set; }
    [field: SerializeField][field: FormerlySerializedAs("Water")] public float Water { get; set; }

    public object Clone()
    {
        return new AgentStats
        {
            Energy = Energy,
            Food = Food,
            Water = Water,
        };
    }
}

public class AiAgent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_navAgent;
    [SerializeField] private Animator m_animator;

    [SerializeField] private AgentStats m_initialStats;

    [Header("Audio")]
    [SerializeField] private AudioSource m_stepSound;
    [SerializeField] private AudioSource m_jumpSound;

    private AiCommandedBehavior _commandedBehvior = new AiCommandedBehavior();

    private AiWanderAroundBehavior _wanderAroundBehavior = new AiWanderAroundBehavior();
    private AiLookForResourcesBehaviour _aiSeekEnergyBehavior = new AiLookForResourcesBehaviour("Energy", 50f, 1f);
    private AiLookForResourcesBehaviour _aiSeekFoodBehavior = new AiLookForResourcesBehaviour("Food", 60f, 0.5f);
    private AiLookForResourcesBehaviour _aiSeekWaterBehavior = new AiLookForResourcesBehaviour("Water", 80f, 2f);

    private IAiBehavior _currentBehaviour;

    private List<IAiBehavior> _behaviourPool = new List<IAiBehavior>();


    private AgentStats _currentStats;
    public float Energy => _currentStats.Energy;

    public bool IsInteracting { get; private set; }

    private void Awake()
    {
        _currentStats = (AgentStats) m_initialStats.Clone();

        SwitchToBehavior(_wanderAroundBehavior);
        _behaviourPool.Add(_wanderAroundBehavior);
        _behaviourPool.Add(_aiSeekEnergyBehavior);
        _behaviourPool.Add(_aiSeekFoodBehavior);
        _behaviourPool.Add(_aiSeekWaterBehavior);
    }

    private void Update()
    {
        //Debug.Log("AI UPDATE");

        _currentStats.Energy -= Time.deltaTime * 5f;
        _currentStats.Food -= Time.deltaTime * 1f;
        _currentStats.Water -= Time.deltaTime * 2f;

        if (_currentStats.Energy < 0) _currentStats.Energy = 0;
        if (_currentStats.Food < 0) _currentStats.Food = 0;
        if (_currentStats.Water < 0) _currentStats.Water = 0;

        var bestBehaviour = FindBestBehaviour();

        if (_currentBehaviour != bestBehaviour)
        {
            SwitchToBehavior(bestBehaviour);
        }

        if (_currentBehaviour.Execute(this))
        {
            bestBehaviour = FindBestBehaviour();
            SwitchToBehavior(bestBehaviour);
        }

        m_animator.SetFloat("Speed", m_navAgent.velocity.magnitude);
    }

    private IAiBehavior FindBestBehaviour()
    {
        float bestPriority = _currentBehaviour.Evaluate(this);
        IAiBehavior bestBehaviour = _currentBehaviour;
        foreach (var aiBehavior in _behaviourPool)
        {
            var priority = aiBehavior.Evaluate(this);
            if (priority > bestPriority)
            {
                bestPriority = priority;
                bestBehaviour = aiBehavior;
            }
        }

        return bestBehaviour;
    }

    public void AddStat(string statName, float amount)
    {
        var property = typeof(AgentStats).GetProperty(statName);
        if (property == null)
        {
            Debug.LogError($"AiAgent: No such stat!: {statName}");
            return;
        }

        var value = (float) property.GetValue(_currentStats);
        value += amount;
        property.SetValue(_currentStats, value);

        Debug.Log($"Added Stat: {value - amount} -> {value}");
    }

    private Dictionary<string, Func<float>> _cachedGetters = new Dictionary<string, Func<float>>();
    internal bool TryGetStat(string statName, out float value)
    {
        if (!_cachedGetters.TryGetValue(statName, out var del))
        {
            var property = typeof(AgentStats).GetProperty(statName);
            if (property == null)
            {
                Debug.LogError($"AiAgent: No such stat!: {statName}");
                value = 0f;
                return false;
            }

            var method = property.GetGetMethod();
            del = (Func<float>)method.CreateDelegate(typeof(Func<float>), _currentStats);

            _cachedGetters.Add(statName, del);
        }

        value = del.Invoke();
        return true;
    }


    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnergyOrb"))
        {
            other.gameObject.SetActive(false);
            _energy += 10f;
            StartCoroutine(Celebarate());
        }
    }
    */


    public void PlayAnimation(string interactionAnimationName)
    {
        StartCoroutine(Internal_PlayAnimationAndWait(interactionAnimationName));
    }

    private IEnumerator Internal_PlayAnimationAndWait(string animationName)
    {
        IsInteracting = true;

        var animationNameHash = Animator.StringToHash(animationName);

        m_navAgent.isStopped = true;
        m_animator.Play(animationNameHash);

        yield return null;
        while (m_animator.GetNextAnimatorStateInfo(0).shortNameHash == animationNameHash
            || m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animationNameHash)
        //while (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Celebrate"))
        {
            yield return null;
        }

        m_navAgent.isStopped = false;

        IsInteracting = false;
    }

    public void SetDestination(Vector3 targetPosition)
    {
        m_navAgent.destination = targetPosition;
    }
    public void SetStoppingDistance(float stoppingDistance)
    {
        m_navAgent.stoppingDistance = stoppingDistance;
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
        m_navAgent.stoppingDistance = 0f;

        _currentBehaviour?.OnExit(this);
        _currentBehaviour = newBehavior;
        _currentBehaviour.OnStart(this);
    }

    private void OnFootStep()
    {
        m_stepSound.volume = Random.Range(0.6f, 0.7f);
        m_stepSound.pitch = Random.Range(0.95f, 1.05f);
        m_stepSound.Play();
    }

    private void OnJump()
    {
        m_jumpSound.volume = Random.Range(0.6f, 0.7f);
        m_jumpSound.pitch = Random.Range(0.95f, 1.05f);
        m_jumpSound.Play();
    }


}
