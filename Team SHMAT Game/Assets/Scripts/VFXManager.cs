using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : SingletonBase<VFXManager>
{
    public Dictionary<string, Effect> VFXList;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

public class Effect
{
    public ParticleSystem system;
    // public 
}