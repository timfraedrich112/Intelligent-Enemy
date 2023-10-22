using BBUnity.Conditions;
using Pada1.BBCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Condition("CustomCondition/ClassIsQueueEmpty")]
public class ClassIsQueueEmpty : GOCondition
{
    public override bool Check()
    {
        return gameObject.GetComponent<AgentAttacks>().attackQueue.Count == 0;
    }
}