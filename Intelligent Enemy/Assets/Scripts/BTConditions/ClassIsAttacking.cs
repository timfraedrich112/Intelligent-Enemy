using BBUnity.Conditions;
using Pada1.BBCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Condition("CustomCondition/ClassIsAttacking")]
public class ClassIsAttacking : GOCondition
{
    public override bool Check()
    {
        return gameObject.GetComponent<AgentAttacks>().isAttacking == true;
    }
}