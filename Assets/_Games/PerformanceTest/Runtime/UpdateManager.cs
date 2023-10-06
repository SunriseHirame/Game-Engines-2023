using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class UpdateManager : MonoBehaviour
{
    private List<Cube> _cubesList = new List<Cube>();
    private List<Updateble> _updatablesList = new List<Updateble>();
    private List<IUpdatable> _iUpdatablesList = new List<IUpdatable>();

    private Cube[] _cubes;
    private Updateble[] _updatables;
    private IUpdatable[] _iUpdatables;


    private void Start()
    {
        _cubes = GameObject.FindObjectsByType<Cube>(FindObjectsSortMode.None);
        _updatables = new Updateble[_cubes.Length];
        _iUpdatables = new IUpdatable[_cubes.Length];

        for (int i = 0; i < _cubes.Length; i++)
        {
            _updatables[i] = _cubes[i];
            _iUpdatables[i] = _cubes[i];
        }

        _cubesList.AddRange(_cubes);
        _updatablesList.AddRange(_cubes);
        _iUpdatablesList.AddRange(_cubes);
    }

    private void Update()
    {
        Profiler.BeginSample("Array::DirectUpdate CACHE");
        foreach (var cube in _cubes)
        {
            cube.DirectUpdate();
        }
        Profiler.EndSample();

        Profiler.BeginSample("Array::DirectUpdate");
        foreach (var cube in _cubes)
        {
            cube.DirectUpdate();
        }
        Profiler.EndSample();
        Profiler.BeginSample("Array::AbstactUpdate");
        foreach (var cube in _updatables)
        {
            cube.AbstractUpdate();
        }
        Profiler.EndSample();
        Profiler.BeginSample("Array::InterfaceUpdate");
        foreach (var cube in _iUpdatables)
        {
            cube.InterfaceUpdate();
        }
        Profiler.EndSample();


        Profiler.BeginSample("List::DirectUpdate");
        foreach (var cube in _cubesList)
        {
            cube.DirectUpdate();
        }
        Profiler.EndSample();
        Profiler.BeginSample("List::AbstactUpdate");
        foreach (var cube in _updatablesList)
        {
            cube.AbstractUpdate();
        }
        Profiler.EndSample();
        Profiler.BeginSample("List::InterfaceUpdate");
        foreach (var cube in _iUpdatablesList)
        {
            cube.InterfaceUpdate();
        }
        Profiler.EndSample();


        var length = 1000;
        Profiler.BeginSample("Vector::Magnitude");
        for (int i = 0; i < length; i++)
        {
            var value = Random.onUnitSphere.magnitude;
            if (value < 0.71f)
            {

            }
        }

        Profiler.EndSample();
        Profiler.BeginSample("Vector::Magnitude");

        for (int i = 0; i < length; i++)
        {
            var sqrValue = Random.onUnitSphere.sqrMagnitude;
            if (sqrValue < 0.71f * 0.71f)
            {

            }
        }
        Profiler.EndSample();

    }
}
