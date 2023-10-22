using BBUnity.Conditions;
using Pada1.BBCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Condition("CustomCondition/ClassUsedRangedLast")]
public class ClassUsedRangedLast : GOCondition
{
    public override bool Check()
    {
        return gameObject.GetComponent<AgentAttacks>().attackQueue[0] == AgentAttacks.Attacks.ranged;
    }
}