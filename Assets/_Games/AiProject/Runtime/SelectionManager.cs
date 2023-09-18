using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    private GameObject _selectedObject;

    // Update is called once per frame
    private void Update()
    {
        var mousePosition = Input.mousePosition;
        var camera = Camera.main;
        var screenToWorldRay = camera.ScreenPointToRay(mousePosition);

        HandleSelection(screenToWorldRay);

        HandleCommanding(screenToWorldRay);
    }

    private void HandleCommanding(Ray screenToWorldRay)
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Command AiAgent to move
            if (_selectedObject == null) return;

            if (_selectedObject.TryGetComponent<AiAgent>(out var aiAgent))
            {
                if (Physics.Raycast(screenToWorldRay, out RaycastHit hit, 1000f))
                {
                    aiAgent.CommandMoveTo(hit.point);
                    Debug.Log($"Commanded agent {aiAgent} to move to: {hit.point}");
                }
            }
        }
    }

    private void HandleSelection(Ray screenToWorldRay)
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Select AiAgent
            if (Physics.Raycast(screenToWorldRay, out RaycastHit hit, 1000f))
            {
                _selectedObject = hit.transform.gameObject;
                Debug.Log($"Selected: {_selectedObject}");
            }
        }
    }
}
