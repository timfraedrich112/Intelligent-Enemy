using BBUnity.Conditions;
using Pada1.BBCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Condition("CustomCondition/ClassInRangeToAttack")]
public class ClassInRangeToAttack : GOCondition
{
    public override bool Check()
    {
        return gameObject.GetComponent<AgentAttacks>().RangeToAttack();
    }
}