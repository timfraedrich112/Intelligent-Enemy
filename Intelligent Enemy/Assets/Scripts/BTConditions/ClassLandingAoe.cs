using BBUnity.Conditions;
using Pada1.BBCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Condition("CustomCondition/ClassLandingAoe")]
public class ClassLandingAoe : GOCondition
{
    public override bool Check()
    {
        AgentMemory memory = gameObject.GetComponent<AgentMemory>();
        return (memory.accuracyThreshold < memory.aoeAccuracy && memory.aoeCount > 3);
    }
}