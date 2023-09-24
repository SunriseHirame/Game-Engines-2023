using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Interatable : MonoBehaviour
{
    public static List<Interatable> All = new List<Interatable>();

    public static bool IsThereAnyWithType (string statName)
    {
        return All.Any(i => i.m_statName == statName);
    }


    [SerializeField] private Transform m_interactionPoint;
    [SerializeField] private float m_interactableRadius;

    [SerializeField] private string m_interactionAnimationName;

    [Space]
    [SerializeField] private bool m_giveStat;
    [SerializeField] private string m_statName;
    [SerializeField] private float m_amount;

    public Vector3 InteractionPoint => m_interactionPoint.position;
    public float InteractableRadius => m_interactableRadius;
    public string InteractionAnimationName => m_interactionAnimationName;


    public bool GivesStats => m_giveStat;
    public string StatName => m_statName;
    public float StatAmount => m_amount;

    private void OnEnable()
    {
        All.Add(this);
    }

    private void OnDisable()
    {
        All.Remove(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(InteractionPoint, InteractableRadius);
    }

    public IEnumerator ReactiveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(true);
        print("Activeated");
    }
}