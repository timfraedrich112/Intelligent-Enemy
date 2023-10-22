using BBUnity.Conditions;
using Pada1.BBCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Condition("CustomCondition/ClassLandingRange")]
public class ClassLandingRange : GOCondition
{
    public override bool Check()
    {
        AgentMemory memory = gameObject.GetComponent<AgentMemory>();
        return (memory.accuracyThreshold < memory.rangedAccuracy && memory.rangedCount > 3);
    }
}