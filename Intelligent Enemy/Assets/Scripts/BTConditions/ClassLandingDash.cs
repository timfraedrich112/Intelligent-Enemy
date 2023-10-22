using BBUnity.Conditions;
using Pada1.BBCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Condition("CustomCondition/ClassLandingDash")]
public class ClassLandingDash : GOCondition
{
    public override bool Check()
    {
        AgentMemory memory = gameObject.GetComponent<AgentMemory>();
        return (memory.accuracyThreshold < memory.dashAccuracy && memory.dashCount > 3);
    }
}