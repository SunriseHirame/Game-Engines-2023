using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IUpdatable
{
    void InterfaceUpdate();
}

public abstract class Updateble : MonoBehaviour
{
    public abstract void AbstractUpdate();
}

public class Cube : Updateble, IUpdatable
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Random.onUnitSphere * Random.value * 10f;
        var propBlock = new MaterialPropertyBlock();
        //propBlock.SetColor("_BaseColor", Random.ColorHSV());
        GetComponent<Renderer>().SetPropertyBlock(propBlock);
    }
    
    void OnDestroy()
    {
        Destroy(GetComponent<Renderer>().material);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DirectUpdate()
    {
    }

    public override void AbstractUpdate()
    {
    }

    public void InterfaceUpdate()
    {
    }
}
